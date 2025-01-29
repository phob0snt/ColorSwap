using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using YG;

public class SecondChanceAD : MonoBehaviour
{
    [SerializeField] private Button _watchAD;
    public event Action OnWatched;
    public event Action OnCanceled;
    private const string AD_ID = "SecondChance";

    private void OnEnable()
    {
        _watchAD.onClick.AddListener(ShowAD);
        StartCoroutine(Countdown());
    }

    private void OnDisable()
    {
        _watchAD.onClick.RemoveListener(ShowAD);
    }

    private IEnumerator Countdown()
    {
        yield return new WaitForSeconds(4);
        OnCanceled.Invoke();
    }

    private void ShowAD()
    {
        YG2.RewardedAdvShow(AD_ID, () => OnWatched.Invoke());
    }
}
