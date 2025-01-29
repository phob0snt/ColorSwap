using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Zenject;

public class ObstaclesHandler : MonoBehaviour
{
    [Inject] private readonly SessionManager _sessionManager;
    [Inject] private readonly SceneObjectFactory _sceneObjectFactory;

    [SerializeField][Range(1, 10)] private float _defaultInterval;
    [SerializeField] private int _scorePointsSpawnRate;
    [SerializeField] private int _repaintSpawnRate;

    [SerializeField] private List<Obstacle> _obstacles = new();
    private List<SceneObject> _interactables = new();

    private float LastObstacleYPos => (_obstacles.Count == 0 ? 0 : _obstacles[^1].transform.position.y);

    private int _scorePointsCounter = 2;
    private int _repaintCounter = 4;

    public ColorPhase LastPhase { get; private set; }

    private const float MIN_INTERVAL = 2.5f;
    private bool _isPlaying = false;

    public void Initialize(ColorPhase startPhase)
    {
        Debug.Log("Obstacles INIT");
        LastPhase = startPhase;
        _isPlaying = true;
        StartCoroutine(SpawnObstacles());
    }

    public void ClearObstacles()
    {
        Debug.Log("Obstacles CLEAR");
        foreach (var obst in _obstacles)
        {
            if (obst != null)
                Destroy(obst.gameObject);
        }
        _obstacles.Clear();
        foreach (var inter in _interactables)
        {
            if (inter != null)
                Destroy(inter.gameObject);
        }
        _interactables.Clear();
        _scorePointsCounter = 2;
        _repaintCounter = 4;
        _isPlaying = false;
    }

    private void CalculateNextScorePoint() => _scorePointsCounter = Random.Range(_scorePointsSpawnRate - 1, _scorePointsSpawnRate + 1);
    private void CalculateNextRepaint() => _repaintCounter = Random.Range(_repaintSpawnRate - 1, _repaintSpawnRate + 1);

    private IEnumerator SpawnObstacles()
    {
        while (_isPlaying)
        {
            if (LastObstacleYPos + CalculateInterval() < Camera.main.transform.position.y + 30)
            {
                if (_scorePointsCounter > 0 && _repaintCounter > 0)
                {
                    SpawnObstacle();
                    _scorePointsCounter--;
                    _repaintCounter--;
                }
                else if (_scorePointsCounter == 0 && _repaintCounter == 0)
                {
                    SpawnScorePoint();
                    CalculateNextScorePoint();
                    _repaintCounter++;
                }
                else if (_repaintCounter == 0)
                {
                    SpawnRepaint();
                    CalculateNextRepaint();
                }
                else
                {
                    SpawnScorePoint();
                    CalculateNextScorePoint();
                    _repaintCounter--;
                }
            }
            yield return new WaitForSeconds(0.2f);
        }
        Debug.Log("SPAWN END");
    }

    private void SpawnScorePoint()
    {
        ScorePoint point = _sceneObjectFactory.GetSceneObject<ScorePoint>(new Vector3(0, LastObstacleYPos + (CalculateInterval() / 2), 0));
        point.OnPick += _sessionManager.IncreaseScore;
        _interactables.Add(point);
    } 

    private float CalculateInterval()
    {
        if (_obstacles.Count == 0) return 1;
        float interval = _defaultInterval + _obstacles[^1].Height;
        if (interval < MIN_INTERVAL) return MIN_INTERVAL;
        else return interval;
    }

    private void ClearOldObstacles()
    {
        List<Obstacle> toRemove = new();
        foreach (var obst in _obstacles)
        {
            if (obst.transform.position.y < Camera.main.transform.position.y - 10)
            {
                toRemove.Add(obst);
                Destroy(obst.gameObject);
            }
        }
        _obstacles.RemoveRange(0, toRemove.Count);
    }

    private void SpawnObstacle()
    {
        Obstacle temp = _sceneObjectFactory.GetSceneObject<Obstacle>(new Vector3(0, LastObstacleYPos + CalculateInterval(), 0), true);
        temp.ConfigurateRandomly(_sessionManager.DifficultyCoef);
        temp.transform.position += new Vector3(0, temp.Height / 2, 0);
        _obstacles.Add(temp);
    }

    private void SpawnRepaint()
    {
        Repaint repaint = _sceneObjectFactory.GetSceneObject<Repaint>(new Vector3(0, LastObstacleYPos + (CalculateInterval() / 2), 0));
        if (repaint.Phase == LastPhase)
        {
            while (repaint.Refresh() == LastPhase) { }
        }
        LastPhase = repaint.Phase;
        _interactables.Add(repaint);
        ClearOldObstacles();
    }
}
