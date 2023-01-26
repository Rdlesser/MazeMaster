using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GridManager : MonoBehaviour
{
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private Tilemap _wallsTilemap;
    [SerializeField] private Tilemap _doorwayTilemap;
    [SerializeField] private Tilemap _playerTilemap;

    private MapData _mapData;
    
    private readonly Dictionary<string, TileBase> _tileDictionary = new Dictionary<string, TileBase>();
    
    private Vector3Int _entrancePosition;
    private Vector3Int _exitPosition;
    private Vector3Int _playerPosition;
    
    private static readonly string Ground = "ground";
    private static readonly string Wall = "wall";
    private static readonly string Entrance = "entrance";
    private static readonly string Exit = "exit";
    private static readonly string Player = "player";

    public Action MapInitted;
    public Action PlayerReachedGoalAction;

    public void Init(MapData mapData)
    {
        _mapData = mapData;
        PreprocessTiles();
        SetGround();
        SetBoundaries();
        SetDoorways();
        DispatchInitComplete();
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
        for (int i = 0; i < _mapData.MapSize.x; i++)
        {
            for (int j = 0; j < _mapData.MapSize.y; j++)
            {
                SetTile(_groundTilemap, new Vector3Int(i, j, 0), _tileDictionary[Ground]);
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
        for (int x = 0; x < _mapData.MapSize.x; x++)
        {
            SetTile(_wallsTilemap, new Vector3Int(x, _mapData.MapSize.y - 1, 0), _tileDictionary[Wall]);
        }
    }

    private void SetLowerBoundaries()
    {
        for (int x = 0; x < _mapData.MapSize.x; x++)
        {
            SetTile(_wallsTilemap, new Vector3Int(x, 0, 0), _tileDictionary[Wall]);
        }
    }

    private void SetLeftBoundaries()
    {
        for (int y = 0; y < _mapData.MapSize.y; y++)
        {
            SetTile(_wallsTilemap, new Vector3Int(0, y, 0), _tileDictionary[Wall]);
        }
    }

    private void SetRightBoundaries()
    {
        
        for (int y = 0; y < _mapData.MapSize.y; y++)
        {
            SetTile(_wallsTilemap, new Vector3Int(_mapData.MapSize.x - 1, y, 0), _tileDictionary[Wall]);
        }
    }

    private void SetDoorways()
    {
        _entrancePosition = GetRandomPosition();
        
        do
        {
            _exitPosition = GetRandomPosition();

        } while (_exitPosition == _entrancePosition);
        
        SetTile(_doorwayTilemap, _entrancePosition, _tileDictionary[Entrance]);
        SetTile(_doorwayTilemap, _exitPosition, _tileDictionary[Exit]);
    }

    private void SetTile(Tilemap tilemap, Vector3Int tilePosition, TileBase tileBase)
    {
        tilemap.SetTile(tilePosition, tileBase);
    }

    private Vector3Int GetRandomPosition()
    {
        var xPosition = Random.Range(1, _mapData.MapSize.x - 2);
        var yPosition = Random.Range(1, _mapData.MapSize.y - 2);
        return new Vector3Int(xPosition, yPosition, 0);
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

        if (IsPlayerAtExit())
        {
            OnPlayerReachedGoal();
        }
    }

    private void UpdatePlayerPosition(Vector3Int movePosition)
    {
        SetTile(_playerTilemap, _playerPosition, null);
        _playerPosition = movePosition;
        SetTile(_playerTilemap, _playerPosition, _tileDictionary[Player]);
    }

    private void OnPlayerReachedGoal()
    {
        PlayerReachedGoalAction?.Invoke();
    }

    private bool IsPlayerAtExit()
    {
        return _playerPosition == _exitPosition;
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
        PlayerReachedGoalAction = null;
    }
}