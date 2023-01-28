using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    [SerializeField] private MapData _mapData;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private CameraSizeFitter _cameraFitter;

    private bool _isGameInProgress = false;
    private int _starsCollected;
    
    private void Awake()
    {
        if (_mapData != null)
        {
            _gridManager.MapInitted += StartGame;
            InitMapManager(_mapData);
            InitCameraFitter(_mapData);
            RegisterToGameEvents();
        }
    }

    private void RegisterToGameEvents()
    {
        GameEventDispatcher.PlayerAtExit += OnPlayerWon;
        GameEventDispatcher.PlayerOnLava += OnPlayerLost;
        GameEventDispatcher.PlayerReachedStar += OnPlayerReachedStar;
    }

    private void OnPlayerReachedStar(int index)
    {
        _starsCollected++;
    }

    private void StartGame()
    {
        _gridManager.MapInitted -= StartGame;
        SpawnPlayer();
    }

    private void InitMapManager(MapData mapData)
    {
        _gridManager.Init(mapData);
    }

    private void InitCameraFitter(MapData mapData)
    {
        _cameraFitter.Init(mapData);
    }

    private void SpawnPlayer()
    {
        _gridManager.SpawnPlayer();
        _isGameInProgress = true;
    }

    public void OnPlayerPressedUp(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }

        _gridManager.MovePlayer(Vector3Int.up);
    }

    public void OnPlayerPressedDown(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }
        
        _gridManager.MovePlayer(Vector3Int.down);
    }

    public void OnPlayerPressedLeft(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }
        
        _gridManager.MovePlayer(Vector3Int.left);
    }

    public void OnPlayerPressedRight(InputAction.CallbackContext context)
    {
        if (!_isGameInProgress || !context.performed)
        {
            return;
        }
        
        _gridManager.MovePlayer(Vector3Int.right);
    }

    private void OnPlayerWon()
    {
        if (_starsCollected < _mapData.StarsAmount)
        {
            return;
        }
        
        _isGameInProgress = false;
        Debug.Log("the player won");
        GameEventDispatcher.DispatchGameEndEvent(true);
    }

    private void OnPlayerLost()
    {
        _isGameInProgress = false;
        Debug.Log("the player lost");
        GameEventDispatcher.DispatchGameEndEvent(false);
    }


    private void UnregisterFromGameEvents()
    {
        GameEventDispatcher.PlayerAtExit -= OnPlayerWon;
        GameEventDispatcher.PlayerOnLava -= OnPlayerLost;
        GameEventDispatcher.PlayerReachedStar += OnPlayerReachedStar;
    }

    private void OnDestroy()
    {
        UnregisterFromGameEvents();
    }
}
