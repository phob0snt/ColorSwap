using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MainView : MonoBehaviour
{
    [Inject] private readonly SessionManager _sessionManager;
    [Inject] private readonly GameManager _gameManager;
    [SerializeField] private TMP_Text _menuDiamonds;
    [SerializeField] private Button _playBtn;
    [SerializeField] private Button _shopBtn;
    [SerializeField] private Slider _volume;

    private void OnEnable()
    {
        _playBtn.onClick.AddListener(StartGame);
    }

    private void OnDisable()
    {
        _playBtn.onClick.RemoveListener(StartGame);
    }
    public void UpdateDiamonds() => _menuDiamonds.text = _gameManager.Diamonds.ToString();

    private void StartGame()
    {
        _sessionManager.StartGame();
        gameObject.SetActive(false);
    }

}


