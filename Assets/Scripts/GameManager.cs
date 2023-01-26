using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    [SerializeField] private Vector2Int _mapSize = new Vector2Int(10, 10);
    // Todo: improve - Make this accept a scriptable object that maps a name to a tile
    [FormerlySerializedAs("_groundTile")] [SerializeField] private List<TileBase> _tileList;
    [SerializeField] private Tilemap _groundTilemap;
    [SerializeField] private Tilemap _wallsTilemap;

    private Dictionary<string, TileBase> _tileDictionary = new Dictionary<string, TileBase>();

    private static string Ground = "ground";
    private static string Wall = "wall";
    private static string Entrance = "entrance";
    private static string Exit = "exit";
    private static string Player = "player";
    
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

    void Update()
    {
        
    }
}
