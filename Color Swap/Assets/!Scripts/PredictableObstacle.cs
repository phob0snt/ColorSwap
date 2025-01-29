using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PredictableObstacle : Obstacle
{
    [SerializeField] private List<ObstaclePart> _playableParts;

    public ColorPhase PlayablePhase { get; set; } = ColorPhase.Red;

    public override void Initialize()
    {
        base.Initialize();
    }
    protected override void ConfigurateParts()
    {
        foreach (var part in _playableParts)
        {
            part.Phase = PlayablePhase;
        }
        var colorPhases = Enum.GetValues(typeof(ColorPhase)).Cast<ColorPhase>().Where(phase => phase != ColorPhase.None && phase != PlayablePhase).ToList();
        var shuffledPhases = colorPhases.OrderBy(_ => UnityEngine.Random.value).ToList();

        List<ColorPhase> shuffledPhases2 = new();
        if (_parts.Count > colorPhases.Count)
        {
            shuffledPhases2 = colorPhases.OrderBy(_ => UnityEngine.Random.value).ToList();
        }

        List<ObstaclePart> parts = _parts.FindAll(x => !_playableParts.Contains(x));
        for (int i = 0; i < parts.Count; i++)
        {
            if (i < colorPhases.Count)
                parts[i].Phase = shuffledPhases[i];
            else
                parts[i].Phase = shuffledPhases2[i - colorPhases.Count];
        }
    }
}
