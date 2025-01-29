using DG.Tweening;
using System;
using UnityEngine;

public class Repaint : SceneObject
{
    [SerializeField] private Animator _animator;
    public ColorPhase Phase { get; private set; }
    private bool _interactable = true;
    public override void Initialize()
    {
        Refresh();
    }

    public ColorPhase Refresh()
    {
        Phase = (ColorPhase)UnityEngine.Random.Range(1, Enum.GetNames(typeof(ColorPhase)).Length);
        return Phase;
    }

    public ColorPhase Interact()
    {
        if (!_interactable) return ColorPhase.None;
        Colors.SetGlowColor(Phase, _animator.gameObject.GetComponent<SpriteRenderer>());
        _interactable = false;
        _animator.gameObject.transform.SetParent(null);
        _animator.Play("Poof");
        transform.DOScale(Vector3.zero, 0.3f).onComplete += () =>
        {
            Destroy(gameObject);
            Destroy(_animator.gameObject);
        };
        _animator.gameObject.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), 0.1f);
        return Phase;
    }
}
