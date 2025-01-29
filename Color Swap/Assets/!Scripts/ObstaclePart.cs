using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D))]
public class ObstaclePart : MonoBehaviour
{
    private SpriteRenderer Renderer => GetComponent<SpriteRenderer>();
    private ColorPhase _phase;
    public ColorPhase Phase
    {
        get { return _phase; }
        set
        {
            Colors.SetGlowColor(value, Renderer);
            _phase = value;
        }
    }
}
