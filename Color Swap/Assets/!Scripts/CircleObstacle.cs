using System;
using UnityEngine;

public class CircleObstacle : Obstacle
{
    public bool ReverseMovement { get; private set; }
    public float RotationSpeed { get; private set; } = 0.6f;
    public override void ConfigurateRandomly(float coef)
    {
        if (coef > MAX_DIFFICULTY_COEF)
            coef = MAX_DIFFICULTY_COEF;
        float sizeCoef = UnityEngine.Random.Range(0.7f, 0.85f) * (2 - coef);
        _height *= coef;
        transform.localScale = new Vector3(sizeCoef, sizeCoef, sizeCoef);
        ReverseMovement = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        RotationSpeed = UnityEngine.Random.Range(0.5f, 0.85f) * coef;
    }

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        if (!ReverseMovement)
        {
            transform.Rotate(0, 0, 1 * RotationSpeed);
        }
        else
        {
            transform.Rotate(0, 0, -1 * RotationSpeed);
        }
    }
}
