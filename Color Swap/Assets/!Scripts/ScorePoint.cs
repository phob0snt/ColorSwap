using DG.Tweening;
using System;
using UnityEngine;

public class ScorePoint : SceneObject
{
    [SerializeField] private Animator _animator;
    private bool _interactable = true;
    public event Action OnPick;

    public void Pick()
    {
        OnPick.Invoke();
        if (!_interactable) return;
        Colors.SetGlowColor(ColorPhase.Purple, _animator.gameObject.GetComponent<SpriteRenderer>());
        _interactable = false;
        _animator.gameObject.transform.SetParent(null);
        _animator.Play("Poof");
        transform.DOScale(Vector3.zero, 0.3f).onComplete += () =>
        {
            Destroy(gameObject);
            Destroy(_animator.gameObject);
        };
        _animator.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.1f);
    }
}
