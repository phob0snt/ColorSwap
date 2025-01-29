using UnityEngine;

public abstract class SceneObject : MonoBehaviour
{
    public float Height => _height;
    [SerializeField] protected float _height;
    public virtual void Initialize() { }
}
