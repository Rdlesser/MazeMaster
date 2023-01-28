
using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _stepsCounter;
    [SerializeField] private TextMeshProUGUI _starsCounter;
    [SerializeField] private GameObject _endGameScreen;
    [SerializeField] private TextMeshProUGUI _victoryStatusText;

    private int _playerSteps;
    private int _playerStars;
    
    private void Start()
    {
        Init();
        RegisterToGameEvents();
    }

    private void RegisterToGameEvents()
    {
        GameEventDispatcher.PlayerTookStep += OnPlayerTookStep;
        GameEventDispatcher.PlayerReachedStar += OnPlayerReachedStar;
        GameEventDispatcher.GameEnd += OnGameEnd;
    }

    private void Init()
    {
        _playerStars = 0;
        _playerSteps = 0;
        UpdatePlayerStarsText();
        UpdatePlayerStepsText();
        _endGameScreen.SetActive(false);
    }

    private void OnPlayerTookStep()
    {
        _playerSteps++;
        UpdatePlayerStepsText();
    }

    private void OnPlayerReachedStar(int index)
    {
        _playerStars++;
        UpdatePlayerStarsText();
    }

    private void UpdatePlayerStarsText()
    {
        _starsCounter.SetText($"Stars: {_playerStars}");
    }

    private void UpdatePlayerStepsText()
    {
        _stepsCounter.SetText($"Steps: {_playerSteps}");
    }

    private void OnGameEnd(bool isVictorious)
    {
        var victoryStatus = isVictorious ? "WIN" : "LOSE";
        var victoryText = $"YOU {victoryStatus}";
        _victoryStatusText.SetText(victoryText);
        _endGameScreen.SetActive(true);
    }

    public void OnStartAgain()
    {
        GameEventDispatcher.DispatchStartAgainEvent();
    }

    private void UnregisterGameEvents()
    {
        GameEventDispatcher.PlayerTookStep -= OnPlayerTookStep;
        GameEventDispatcher.PlayerReachedStar -= OnPlayerReachedStar;
        GameEventDispatcher.GameEnd -= OnGameEnd;
    }

    private void OnDestroy()
    {
        UnregisterGameEvents();
    }
}
