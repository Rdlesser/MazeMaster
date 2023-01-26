using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Vector2Int _mapSize = new Vector2Int(10, 10);
    // Todo: improve - Make this accept a scriptable object that maps a name to a tile
    [SerializeField] private MazeTilesList _mazeTilesList;
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private Tilemap _wallsTilemap;
    [SerializeField] private Tilemap _doorwayTilemap;
    [SerializeField] private Tilemap _playerTilemap;

    private readonly Dictionary<string, TileBase> _tileDictionary = new Dictionary<string, TileBase>();
    
    private Vector3Int _entrancePosition;
    private Vector3Int _exitPosition;
    private Vector3Int _playerPosition;


    private static readonly string Ground = "ground";
    private static readonly string Wall = "wall";
    private static readonly string Entrance = "entrance";
    private static readonly string Exit = "exit";
    private static readonly string Player = "player";

    private bool _isGameInProgress = false;
    
    private void Awake()
    {
        PreprocessTiles();
    }

    private void PreprocessTiles()
    {
        foreach (var tileData in _mazeTilesList.TileDatas)
        {
            _tileDictionary[tileData.Name.ToLower()] = tileData.Tile;
        }
        
    }

    void Start()
    {
        SetGround();
        SetBoundaries();
        SetDoorways();
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        _playerTilemap.ClearAllTiles();
        _playerPosition = _entrancePosition;
        _playerTilemap.SetTile(_entrancePosition, _tileDictionary[Player]);
        _playerPosition = _entrancePosition;
        _isGameInProgress = true;
    }


    private void SetGround()
    {
        for (int i = 0; i < _mapSize.x; i++)
        {
            for (int j = 0; j < _mapSize.y; j++)
            {
                var tilePosition = new Vector3Int(i, j, 0);
                _groundTilemap.SetTile(tilePosition, _tileDictionary[Ground]);
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
        for (int x = 0; x < _mapSize.x; x++)
        {
            var tilePosition = new Vector3Int(x, _mapSize.y - 1, 0);
            _wallsTilemap.SetTile(tilePosition, _tileDictionary[Wall]);
        }
    }

    private void SetLowerBoundaries()
    {
        for (int x = 0; x < _mapSize.x; x++)
        {
            var tilePosition = new Vector3Int(x, 0, 0);
            _wallsTilemap.SetTile(tilePosition, _tileDictionary[Wall]);
        }
    }

    private void SetLeftBoundaries()
    {
        for (int y = 0; y < _mapSize.y; y++)
        {
            var tilePosition = new Vector3Int(0, y, 0);
            _wallsTilemap.SetTile(tilePosition, _tileDictionary[Wall]);
        }
    }

    private void SetRightBoundaries()
    {
        for (int y = 0; y < _mapSize.y; y++)
        {
            var tilePosition = new Vector3Int(_mapSize.x - 1, y, 0);
            _wallsTilemap.SetTile(tilePosition, _tileDictionary[Wall]);
        }
    }

    private void SetDoorways()
    {
        _entrancePosition = GetRandomPosition();

        // TODO: Possibility of an infinite loop - search what can be done
        do
        {
            _exitPosition = GetRandomPosition();

        } while (_exitPosition == _entrancePosition);
        
        _doorwayTilemap.SetTile(_entrancePosition, _tileDictionary[Entrance]);
        _doorwayTilemap.SetTile(_exitPosition, _tileDictionary[Exit]);
    }
    
    private Vector3Int GetRandomPosition()
    {
        var xPosition = Random.Range(1, _mapSize.x - 2);
        var yPosition = Random.Range(1, _mapSize.y - 2);
        return new Vector3Int(xPosition, yPosition, 0);
    }

    public void OnPlayerPressUp(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        var movePosition = _playerPosition;
        movePosition.y++;
        if (IsPositionBlocked(movePosition))
        {
            Debug.Log("Blocked");
            return;
        }

        MovePlayer(movePosition);
    }
    
    public void OnPlayerPressDown(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        var movePosition = _playerPosition;
        movePosition.y--;
        if (IsPositionBlocked(movePosition))
        {
            Debug.Log("Blocked");
            return;
        }

        MovePlayer(movePosition);
    }
    
    public void OnPlayerPressLeft(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        var movePosition = _playerPosition;
        movePosition.x--;
        if (IsPositionBlocked(movePosition))
        {
            Debug.Log("Blocked");
            return;
        }

        MovePlayer(movePosition);
    }
    
    public void OnPlayerPressRight(InputAction.CallbackContext context)
    {
        if (!context.performed)
        {
            return;
        }
        
        var movePosition = _playerPosition;
        movePosition.x++;
        
        if (IsPositionBlocked(movePosition))
        {
            return;
        }

        MovePlayer(movePosition);
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

    private void MovePlayer(Vector3Int movePosition)
    {
        if (!_isGameInProgress)
        {
            return;
        }
        
        _playerTilemap.SetTile(_playerPosition, null);
        _playerPosition = movePosition;
        _playerTilemap.SetTile(_playerPosition, _tileDictionary[Player]);
        if (IsPlayerAtExit())
        {
            OnPlayerWon();
        }
    }

    private void OnPlayerWon()
    {
        _isGameInProgress = false;
        Debug.Log("the player won");
    }

    private bool IsPlayerAtExit()
    {
        return _playerPosition == _exitPosition;
    }
}
