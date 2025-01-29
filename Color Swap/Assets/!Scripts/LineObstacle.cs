using System;
using UnityEngine;

public class LineObstacle : Obstacle
{
    public bool ReverseMovement { get; private set; }
    public float MoveSpeed { get; private set; } = 1.7f;

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        foreach (var part in _parts)
        {
            if (!ReverseMovement)
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

    public override void ConfigurateRandomly(float coef)
    {
        if (coef > MAX_DIFFICULTY_COEF)
            coef = MAX_DIFFICULTY_COEF;
        ReverseMovement = Convert.ToBoolean(UnityEngine.Random.Range(0, 2));
        MoveSpeed = UnityEngine.Random.Range(1.7f, 2.35f) * coef;
    }
}
