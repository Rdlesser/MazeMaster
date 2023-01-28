
using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI _stepsCounter;
    [SerializeField] private TextMeshProUGUI _starsCounter;

    private int _playerSteps;
    private int _playerStars;
    
    private void Start()
    {
        Init();
        GameEventDispatcher.PlayerTookStep += OnPlayerTookStep;
        GameEventDispatcher.PlayerReachedStar += OnPlayerReachedStar;
    }

    private void Init()
    {
        _playerStars = 0;
        _playerSteps = 0;
        UpdatePlayerStarsText();
        UpdatePlayerStepsText();
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

    private void OnDestroy()
    {
        GameEventDispatcher.PlayerTookStep -= OnPlayerTookStep;
        GameEventDispatcher.PlayerReachedStar -= OnPlayerReachedStar;
    }
}
