using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{

    [SerializeField] private MapData _mapData;
    [SerializeField] private MapManager _mapManager;
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
        if (_mapData != null)
        {
            _mapManager.MapInitted += SpawnPlayer;
            InitMapManager(_mapData);
        }
    }

    private void InitMapManager(MapData mapData)
    {
        _mapManager.Init(mapData);
        _mapManager.PlayerReachedGoalAction += OnPlayerWon;
    }

    private void SpawnPlayer()
    {
        _mapManager.SpawnPlayer();
        _isGameInProgress = true;
    }
    
    public void OnPlayerPressedUp(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }

        _mapManager.MovePlayer(Vector3Int.up);
    }
    
    public void OnPlayerPressDown(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }
        
        _mapManager.MovePlayer(Vector3Int.down);
    }
    
    public void OnPlayerPressLeft(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }
        
        _mapManager.MovePlayer(Vector3Int.left);
    }
    
    public void OnPlayerPressRight(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }
        
        _mapManager.MovePlayer(Vector3Int.right);
    }

    private void OnPlayerWon()
    {
        _isGameInProgress = false;
        Debug.Log("the player won");
    }

}
