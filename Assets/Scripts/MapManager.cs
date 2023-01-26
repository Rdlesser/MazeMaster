using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
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

    public void Init(MapData mapData)
    {
        
    }
    
    private void PreprocessTiles(List<TileBase> tileList)
    {
        foreach (var tile in tileList)
        {
            _tileDictionary[tile.name.ToLower()] = tile;
        }
    }
}