using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private Tilemap _wallsTilemap; 
    [SerializeField] private Tilemap _overlayTilemap;
    [SerializeField] private Tilemap _playerTilemap;

    private MapData _mapData;

    private Pathfinding _pathfinding;
    private Grid<PathNode> _grid;
    private List<PathNode> _emptyPathNodes;

    private readonly Dictionary<string, Tile> _tileDictionary = new Dictionary<string, Tile>();
    
    private Vector3Int _entrancePosition;
    private Vector3Int _exitPosition;
    private Vector3Int _playerPosition;
    private List<Vector3Int> _starPositions;

    private static readonly string Ground = "ground";
    private static readonly string Wall = "wall";
    private static readonly string Entrance = "entrance";
    private static readonly string Exit = "exit";
    private static readonly string Player = "player";
    private static readonly string Lava = "lava";
    private static readonly string Star = "star";

    public Action MapInitted;

    public void Init(MapData mapData)
    {
        _mapData = mapData;
        PreprocessTiles();
        SetGround();
        InstantiatePathfinding();
        SetBoundaries();
        SetDoorways();
        SetStars();
        SetExtraWalls();
        SetLavaTiles();
        CenterGameOnScreen();
        DispatchInitComplete();
    }

    private void CenterGameOnScreen()
    {
        var sizeX = _mapData.MapSize.x;
        var sizeY = _mapData.MapSize.y;
        transform.position = new Vector3(sizeX / -2f, sizeY / -2f, 0);
    }

    private void PreprocessTiles()
    {
        foreach (var tile in _mapData.MazeTilesList.TileDatas)
        {
            _tileDictionary[tile.Name.ToLower()] = tile.Tile;
        }
    }

    private void SetGround()
    {
        for (var i = 0; i < _mapData.MapSize.x; i++)
        {
            for (var j = 0; j < _mapData.MapSize.y; j++)
            {
                SetTile(_groundTilemap, new Vector3Int(i, j, 0), _tileDictionary[Ground]);
            }
        }
    }

    private void InstantiatePathfinding()
    {
        _pathfinding = new Pathfinding(_mapData.MapSize.x, _mapData.MapSize.y);
        _grid = _pathfinding.GetGrid();
        InstantiateFreeNodesList();
        
    }

    private void InstantiateFreeNodesList()
    {
        _emptyPathNodes = new List<PathNode>();
        for (var x = 0; x < _grid.GetWidth(); x++)
        {
            for (var y = 0; y < _grid.GetHeight(); y++)
            {
                _emptyPathNodes.Add(_grid.GetGridObject(x, y));
            }
        }
    }

    private void SetBoundaries()
    {
        SetUpperBoundaries();
        SetLowerBoundaries();
        SetLeftBoundaries();
        SetRightBoundaries();
    }

    private void SetUpperBoundaries()
    {
        for (var x = 0; x < _mapData.MapSize.x; x++)
        {
            SetTile(_wallsTilemap, new Vector3Int(x, _mapData.MapSize.y - 1, 0), _tileDictionary[Wall]);
            UpdateGridStatus(x, _mapData.MapSize.y - 1, NodeStatus.Blocked);
        }
    }

    private void SetLowerBoundaries()
    {
        for (var x = 0; x < _mapData.MapSize.x; x++)
        {
            SetTile(_wallsTilemap, new Vector3Int(x, 0, 0), _tileDictionary[Wall]);
            UpdateGridStatus(x, 0, NodeStatus.Blocked);
        }
    }

    private void SetLeftBoundaries()
    {
        for (var y = 0; y < _mapData.MapSize.y; y++)
        {
            SetTile(_wallsTilemap, new Vector3Int(0, y, 0), _tileDictionary[Wall]);
            UpdateGridStatus(0, y, NodeStatus.Blocked);
        }
    }

    private void SetRightBoundaries()
    {
        
        for (var y = 0; y < _mapData.MapSize.y; y++)
        {
            SetTile(_wallsTilemap, new Vector3Int(_mapData.MapSize.x - 1, y, 0), _tileDictionary[Wall]);
            UpdateGridStatus(_mapData.MapSize.x - 1, y, NodeStatus.Blocked);
        }
    }

    private void SetDoorways()
    {
        _entrancePosition = GetRandomEmptyPosition();
        
        do
        {
            _exitPosition = GetRandomEmptyPosition();

        } while (_exitPosition == _entrancePosition);
        
        SetTile(_overlayTilemap, _entrancePosition, _tileDictionary[Entrance]);
        UpdateGridStatus(_entrancePosition.x, _entrancePosition.y, NodeStatus.Traversable);
        SetTile(_overlayTilemap, _exitPosition, _tileDictionary[Exit]);
        UpdateGridStatus(_exitPosition.x, _exitPosition.y, NodeStatus.Traversable);
    }

    private void UpdateGridStatus(int x, int y, NodeStatus nodeStatus)
    {
        var gridObject = _grid.GetGridObject(x, y);
        gridObject.NodeStatus = nodeStatus;
        _emptyPathNodes.Remove(gridObject);
    }
    
    private void SetStars()
    {
        _starPositions = new List<Vector3Int>();
        for (int i = 0; i < _mapData.StarsAmount; i++)
        {
            var position = GetRandomEmptyPosition();
            _starPositions.Add(position);
            // TODO: Find a way to generalize the star process such that I won't need a separate StarTile asset for each star
            var tile = (StarTile) _tileDictionary[$"{Star}{i+1}"];
            tile.Index = i;
            SetTile(_overlayTilemap, position, tile);
            UpdateGridStatus(position.x, position.y, NodeStatus.Traversable);
        }

        GameEventDispatcher.PlayerReachedStar += OnPlayerReachedStar;
    }

    private void OnPlayerReachedStar(int index)
    {
        var position = _starPositions[index];
        SetTile(_overlayTilemap, position, null);
        UpdateGridStatus(position.x, position.y, NodeStatus.Empty);
    }

    private void SetExtraWalls()
    {
        // TODO: this is a greedy algorithm and can be improved using recursion
        for (var i = 0; i < _mapData.ExtraWalls; i++)
        {
            if (!TrySetTileAtRandom(_wallsTilemap, _tileDictionary[Wall]))
            {
                Debug.LogError($"Could not set all extra walls - only added {i-1}");
            }
        }
        
    }

    private void SetLavaTiles()
    {
        // TODO: this is a greedy algorithm and can be improved using recursion
        for (var i = 0; i < _mapData.LavaTileCount; i++)
        {
            if (!TrySetTileAtRandom(_overlayTilemap, _tileDictionary[Lava]))
            {
                Debug.LogError($"Could not set all laval tiles - only added {i-1}");
            }
        }
    }

    private bool  TrySetTileAtRandom(Tilemap tilemap, Tile tile)
    {
        var randomEmptyPosition = GetRandomNonBlockingEmptyPosition();
        if (randomEmptyPosition == null)
        {
            return false;
        }

        var emptyPosition = (Vector3Int)randomEmptyPosition;
        
        SetTile(tilemap, emptyPosition, tile);
        UpdateGridStatus(emptyPosition.x, emptyPosition.y, NodeStatus.Blocked);
        
        return true;
    }

    private void SetTile(Tilemap tilemap, Vector3Int tilePosition, Tile tile)
    {
        tilemap.SetTile(tilePosition, tile);
    }

    private Vector3Int? GetRandomNonBlockingEmptyPosition()
    {
        var indexList = GetShuffledIndexList();

        foreach (var index in indexList)
        {
            var node = _emptyPathNodes[index];
            if (IsPossibleBlockingPosition(node))
            {
                return new Vector3Int(node.X, node.Y, 0);
            }
        }

        return null;
    }

    private bool IsPossibleBlockingPosition(PathNode node)
    {
        var ans = false;
        node.SetNodeStatus(NodeStatus.Blocked);
        if (_pathfinding.FindPath(_entrancePosition.x, _entrancePosition.y, _exitPosition.x, _exitPosition.y) != null)
        {
            ans = true;
            foreach (var starPosition in _starPositions)
            {
                if (_pathfinding.FindPath(_entrancePosition.x, _entrancePosition.y, starPosition.x, starPosition.y) == null)
                {
                    ans = false;
                    break;
                }
            }
        }
        
        node.SetNodeStatus(NodeStatus.Empty);
        return ans;
    }

    private List<int> GetShuffledIndexList()
    {
        var indexList = new List<int>();
        for (var i = 0; i < _emptyPathNodes.Count; i++)
        {
            indexList.Add(i);
        }

        Misc.ShuffleIntList(ref indexList);

        return indexList;
    }

    private Vector3Int GetRandomEmptyPosition()
    {
        var randomIndex = Random.Range(0, _emptyPathNodes.Count);
        var node = _emptyPathNodes[randomIndex];
        return new Vector3Int(node.X, node.Y, 0);
    }


    private bool IsPositionBlocked(Vector3Int movePosition)
    {
        var tile = _wallsTilemap.GetTile(movePosition);
        
        if (tile != null && tile.name.Equals(Wall, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return false;
    }

    public void MovePlayer(Vector3Int moveDirection)
    {
        var movePosition = _playerPosition + moveDirection;
        
        if (IsPositionBlocked(movePosition))
        {
            return;
        }
        
        UpdatePlayerPosition(movePosition);
        InteractWithTile();
    }

    private void InteractWithTile()
    {
        var tile = _overlayTilemap.GetTile(_playerPosition);
        var mazeTile = (MazeTile)tile;
        if (mazeTile != null)
        {
            mazeTile.Interact();
        }
    }

    private void UpdatePlayerPosition(Vector3Int movePosition)
    {
        SetTile(_playerTilemap, _playerPosition, null);
        _playerPosition = movePosition;
        SetTile(_playerTilemap, _playerPosition, _tileDictionary[Player]);
    }
    
    private void DispatchInitComplete()
    {
        MapInitted?.Invoke();
    }

    public void SpawnPlayer()
    {
        _playerTilemap.ClearAllTiles();
        SetTile(_playerTilemap, _entrancePosition, _tileDictionary[Player]);
        _playerPosition = _entrancePosition;

    }

    private void OnDestroy()
    {
        MapInitted = null;
        GameEventDispatcher.PlayerReachedStar -= OnPlayerReachedStar;
    }
}