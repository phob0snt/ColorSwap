using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource _musicSource;
    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _jumpSource;
    [SerializeField] private List<AudioClip> _sfxClips;
    [SerializeField] private List<AudioClip> _musicClips;

    private Dictionary<SFX, AudioClip> _sfx;
    private Dictionary<Music, AudioClip> _music;

    private void Awake()
    {
        _sfx = new()
        {
            { SFX.Jump, _sfxClips[0] },
            { SFX.ScorePoint, _sfxClips[1] },
            { SFX.Repaint, _sfxClips[2] },
            { SFX.Death, _sfxClips[3] }
        };

        _music = new()
        {
            { Music.Game, _sfxClips[0] },
            { Music.Menu, _sfxClips[1] }
        };
    }
    public void PlayMusic(Music music)
    {
        _musicSource.clip = _music[music];
        _musicSource.Play();
    }
    public void PlaySound(SFX sfx)
    {
        if (sfx == SFX.Jump)
        {
            _jumpSource.clip = _sfx[sfx];
            _jumpSource.Play();
            return;
        }
        _sfxSource.clip = _sfx[sfx];
        _sfxSource.Play();
    }
}

public enum SFX
{
    Jump,
    ScorePoint,
    Repaint,
    Death
}

public enum Music
{
    Menu,
    Game
}
