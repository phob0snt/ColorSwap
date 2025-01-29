using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;

public abstract class Obstacle : SceneObject
{
    protected List<ObstaclePart> _parts = new();
    protected const float MAX_DIFFICULTY_COEF = 1.4f;
    public override void Initialize()
    {
        ObstaclePart[] parts = transform.GetComponentsInChildren<ObstaclePart>();
        foreach (var part in parts)
            _parts.Add(part);
        if (_parts.Count < Enum.GetNames(typeof(ColorPhase)).Length - 1)
            throw new Exception("Частей препятствия не может быть меньше чем цветов в игре");
        ConfigurateParts();
    }

    public abstract void ConfigurateRandomly(float difficultyCoeff);

    protected virtual void ConfigurateParts()
    {
        var colorPhases = Enum.GetValues(typeof(ColorPhase)).Cast<ColorPhase>().Where(phase => phase != ColorPhase.None).ToList();
        var shuffledPhases = colorPhases.OrderBy(_ => UnityEngine.Random.value).ToList();
        for (int i = 0; i < _parts.Count; i++)
        {
            _parts[i].Phase = shuffledPhases[i];
            Debug.Log(shuffledPhases[i]);
        }
    }
}
