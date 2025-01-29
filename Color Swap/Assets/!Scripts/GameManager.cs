using UnityEngine;
using Zenject;

public class GameManager : MonoBehaviour
{
    [Inject] private readonly SessionManager _sessionManager;
    public int Diamonds { get; private set; }

    private void OnEnable()
    {
        _sessionManager.OnScoreIncrease += () => Diamonds++;
    }
    private void OnDisable()
    {
        _sessionManager.OnScoreIncrease -= () => Diamonds++;
    }
}
