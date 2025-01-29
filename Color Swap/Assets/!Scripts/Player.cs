using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using Zenject;
using YG;
using TMPro;
using DG.Tweening;

public enum ColorPhase
{
    None,
    Red,
    Purple,
    Yellow,
    Blue,
    Green
}
public static class Colors
{
    private static Dictionary<ColorPhase, Color> ColorRefs = new()
    {
        { ColorPhase.None, Color.white },
        { ColorPhase.Red, new Color(0.85f, 0, 0.00757f) },
        { ColorPhase.Purple, new Color(0.5f, 0, 0.5f) },
        { ColorPhase.Yellow, new Color(0.877f, 0.80745f, 0f) },
        { ColorPhase.Blue, new Color(0, 0f, 0.9f) },
        { ColorPhase.Green, new Color(0, 0.75f, 0f) }
    };

    public static void SetGlowColor(ColorPhase phase, Renderer renderer)
    {
        Color col = ColorRefs[phase];
        Color.RGBToHSV(col, out float H, out float S, out float V);
        S -= 0.15f;
        renderer.material.color = Color.HSVToRGB(H, S, V);
        renderer.material.EnableKeyword("_EMISSION");
        Color finalColor = Color.black;
        if (phase != ColorPhase.Purple)
        {
            finalColor = col * 1.5f;
        }
        else
        {
            finalColor = col * 2.5f;
        }
        renderer.material.SetColor("_EmissionColor", finalColor);
    }

    public static ColorPhase GetRandomColorPhase() => (ColorPhase)UnityEngine.Random.Range(1, Enum.GetNames(typeof(ColorPhase)).Length);
}


[RequireComponent(typeof(SpriteRenderer), typeof (Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Inject] private readonly AudioManager _audioManager;
    [Inject] private readonly SessionManager _sessionManager;
    [SerializeField] private InputAction _jumpAction;
    [SerializeField] private Animator _animator;
    [SerializeField] private TMP_Text _diamondsCount; 
    [SerializeField] private SecondChanceAD _AD;

    public SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
    private Rigidbody2D Rb => GetComponent<Rigidbody2D>();
    public ColorPhase Phase { get; private set; } = ColorPhase.None;

    private bool _wasSaved = false;
    private bool _isActive = true;

    private void Awake()
    {
        Colors.SetGlowColor(ColorPhase.None, Renderer);
        Phase = ColorPhase.None;
    }

    public void StartGame(ColorPhase startPhase)
    {
        Debug.Log("PLAYER START GAME");
        _isActive = true;
        _diamondsCount.gameObject.SetActive(true);
        StartCoroutine(AnimateStartColorPick(startPhase));
    }

    public void DisplayDiamonds(int score)
    {
        _diamondsCount.text = score.ToString();
    }
    
    public void BackToMenu()
    {
        transform.position = Vector3.down * 10;
        Colors.SetGlowColor(ColorPhase.None, Renderer);
        Phase = ColorPhase.None;
        transform.DOMoveY(-3, 1.5f).SetEase(Ease.InOutQuad);
    }

    private void Update()
    {
        if (Camera.main.transform.position.y < transform.position.y && _isActive)
            Camera.main.transform.position = new Vector3(0, transform.position.y, -10);
    }

    private void Repaint(ColorPhase phase)
    {
        Colors.SetGlowColor(phase, Renderer);
        Phase = phase;
        _audioManager.PlaySound(SFX.Repaint);
    }

    private void Jump()
    {
        Rb.linearVelocityY *= 0.05f;
        Rb.AddForceY(5, ForceMode2D.Impulse);
        _audioManager.PlaySound(SFX.Jump);
        _animator.Play("Jump", 0, 0f);
    }

    private IEnumerator AnimateStartColorPick(ColorPhase startPhase)
    {
        ColorPhase phase = Colors.GetRandomColorPhase();
        ColorPhase prev = ColorPhase.None;
        float currentInterval = 0.08f;
        float intervalMultiplier = 1.3f;
        int colorsToSwap = 10;
        while (colorsToSwap > 0)
        {
            if (colorsToSwap == 1)
                phase = startPhase;
            else
            {
                phase = Colors.GetRandomColorPhase();
                while (phase == prev)
                    phase = Colors.GetRandomColorPhase();
            }
            prev = phase;
            Colors.SetGlowColor(phase, Renderer);
            _audioManager.PlaySound(SFX.Repaint);
            yield return new WaitForSeconds(currentInterval);
            currentInterval *= intervalMultiplier;
            colorsToSwap--;
        }
        Phase = phase;
        EnableControls();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive) return;
        if (collision.TryGetComponent(out ObstaclePart part))
        {
            if (part.Phase != Phase)
                Lose();
        }
        else if (collision.TryGetComponent(out Repaint repaint))
        {
            ColorPhase phase = repaint.Interact();
            if (phase != ColorPhase.None)
                Repaint(phase);
        }
        else if (collision.TryGetComponent(out ScorePoint point))
        {
            point.Pick();
            _audioManager.PlaySound(SFX.ScorePoint);
        }
        else if (collision.CompareTag("PlayerDeadzone"))
            Lose();
    }

    private void Lose()
    {
        _isActive = false;
        Camera.main.transform.SetParent(null);
        _jumpAction.Disable();
        Rb.linearVelocity = Vector3.zero;
        Rb.gravityScale = 0f;
        if (!_wasSaved)
        {
            _AD.gameObject.SetActive(true);
            _AD.OnWatched += SecondChance;
            _AD.OnCanceled += () => {
                _AD.gameObject.SetActive(false);
                _diamondsCount.gameObject.SetActive(false);
                _sessionManager.GoToMenu();
            };
        }
        else
        {
            _diamondsCount.gameObject.SetActive(false);
            _sessionManager.GoToMenu();
            YG2.InterstitialAdvShow();
        }
    }

    private void SecondChance()
    {
        _AD.gameObject.SetActive(false);
        _wasSaved = true;
    }

    private void EnableControls()
    {
        Rb.gravityScale = 1.0f;
        _jumpAction.Enable();
        _jumpAction.performed += (ctx) => Jump();
    }
}
