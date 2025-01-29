using System;
using System.Collections.Generic;
using UnityEngine;

public class DoubleLineObstacle : PredictableObstacle
{
    public bool ReverseMovement { get; private set; }
    public float MoveSpeed { get; private set; } = 1.5f;
    private List<ObstaclePart> first = new();
    private List<ObstaclePart> second = new();
    public override void ConfigurateRandomly(float coef)
    {
        if (coef > MAX_DIFFICULTY_COEF)
            coef = MAX_DIFFICULTY_COEF;
        ReverseMovement = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        MoveSpeed = UnityEngine.Random.Range(1.5f, 2.1f) * coef;
        for (int i = 0; i < _parts.Count; i++)
        {
            if (i < 5)
                first.Add(_parts[i]);
            else
                second.Add(_parts[i]);
        }
    }

    private void Update()
    {
        if (first != null && second != null)
        {
            Move(first, ReverseMovement);
            Move(second, !ReverseMovement);
        }
    }

    private void Move(List<ObstaclePart> parts, bool reverse)
    {
        foreach (var part in parts)
        {
            if (!reverse)
            {
                part.transform.position += MoveSpeed * Time.deltaTime * Vector3.right;
                if (part.transform.position.x > 3.75f)
                    part.transform.position -= new Vector3(7.5f, 0, 0);
            }
            else
            {
                part.transform.position -= Vector3.right * MoveSpeed * Time.deltaTime;
                if (part.transform.position.x < -3.75f)
                    part.transform.position += new Vector3(7.5f, 0, 0);
            }
        }
    }
}
