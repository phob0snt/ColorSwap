using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class SceneObjectFactory : MonoBehaviour
{
    [Inject] private readonly ObstaclesHandler _handler;
    [SerializeField] private List<SceneObject> _sceneObjects;
    public T GetSceneObject<T>(Vector3 position, bool random=false) where T : SceneObject
    {
        T obj = null;
        if (random)
        {
            List<T> objs = _sceneObjects.OfType<T>().ToList();
            obj = objs[Random.Range(0, objs.Count)];
        }
        else
            obj = _sceneObjects.FirstOrDefault(x => x is T) as T;

        if (obj != null)
        {
            obj = Instantiate(obj, position, Quaternion.identity);
            if (obj is PredictableObstacle predict)
                predict.PlayablePhase = _handler.LastPhase;
            obj.Initialize();
            return obj;
        }
        else
        {
            Debug.LogAssertion($"no object of type {typeof(T)} was found");
            return null;
        }
    }
}
