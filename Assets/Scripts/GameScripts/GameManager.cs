using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{

    [SerializeField] private MapData _mapData;
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private CameraSizeFitter _cameraFitter;

    private bool _isGameInProgress = false;
    
    private void Awake()
    {
        if (_mapData != null)
        {
            _gridManager.MapInitted += StartGame;
            InitMapManager(_mapData);
            InitCameraFitter(_mapData);
        }
    }

    private void StartGame()
    {
        SpawnPlayer();
    }

    private void InitMapManager(MapData mapData)
    {
        _gridManager.Init(mapData);
        _gridManager.PlayerReachedGoalAction += OnPlayerWon;
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
        _isGameInProgress = false;
        Debug.Log("the player won");
    }

}
