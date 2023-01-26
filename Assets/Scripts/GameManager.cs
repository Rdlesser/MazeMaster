using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Vector2Int _mapSize = new Vector2Int(10, 10);
    // Todo: improve - Make this accept a scriptable object that maps a name to a tile
    [FormerlySerializedAs("_groundTile")] [SerializeField] private List<TileBase> _tileList;
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
    
    private void Awake()
    {
        PreprocessTiles();
    }

    private void PreprocessTiles()
    {
        foreach (var tile in _tileList)
        {
            _tileDictionary[tile.name.ToLower()] = tile;
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
}
