using DG.Tweening;
using System;
using TMPro;
using UnityEngine;
using Zenject;

public class SessionManager : MonoBehaviour
{
    public event Action OnScoreIncrease;

    [Inject] private readonly ObstaclesHandler _handler;
    public Player Player => _player;
    [SerializeField] private Player _player;
    [SerializeField] private MainView _mainView;
    private int _score;
    public float DifficultyCoef => 1 + _score * 0.02f;

    public void IncreaseScore()
    {
        _score++;
        _player.DisplayDiamonds(_score);
        OnScoreIncrease.Invoke();
    }
    public void StartGame()
    {
        Debug.Log("SESSION START GAME");
        ColorPhase startPhase = Colors.GetRandomColorPhase();
        _player.StartGame(startPhase);
        _handler.Initialize(startPhase);
    }

    public void GoToMenu()
    {
        Debug.Log("SESSION GO TO MENU");
        _handler.ClearObstacles();
        _mainView.gameObject.SetActive(true);
        _mainView.UpdateDiamonds();
        Camera.main.transform.position = new Vector3(0, 0, -10);
        _player.BackToMenu();
    }
}
