using System;
using UnityEngine;

public class DoubleCircle : PredictableObstacle
{
    public bool ReverseMovement { get; private set; }
    public float RotationSpeed { get; private set; } = 0.6f;
    private Transform first;
    private Transform second;
    public override void ConfigurateRandomly(float coef)
    {
        first = transform.GetChild(0);
        second = transform.GetChild(1);
        if (coef > MAX_DIFFICULTY_COEF)
            coef = MAX_DIFFICULTY_COEF;
        float outerSizeCoef = UnityEngine.Random.Range(0.7f, 0.85f) * (2.05f - coef);
        _height *= coef * 1.8f;
        first.localScale = Vector3.one * outerSizeCoef;
        second.localScale = Vector3.one * (outerSizeCoef + 0.15f);
        ReverseMovement = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        RotationSpeed = UnityEngine.Random.Range(0.55f, 0.70f) * coef;
    }

    private void Update()
    {
        if (first != null && second != null)
        {
            Rotate(first, ReverseMovement);
            Rotate(second, !ReverseMovement);
        }
    }

    private void Rotate(Transform circle, bool reverse)
    {
        if (!reverse)
        {
            circle.Rotate(0, 0, 1 * RotationSpeed);
        }
        else
        {
            circle.Rotate(0, 0, -1 * RotationSpeed);
        }
    }
}
