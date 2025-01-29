using UnityEngine;
using Zenject;

public class DependenciesInstaller : MonoInstaller
{
    [SerializeField] private ObstaclesHandler _handler;
    [SerializeField] private SessionManager _session;
    [SerializeField] private SceneObjectFactory _factory;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private GameManager _gameManager;

    public override void InstallBindings()
    {
        Container.Bind<ObstaclesHandler>().FromInstance(_handler).AsSingle();
        Container.Bind<SessionManager>().FromInstance(_session).AsSingle();
        Container.Bind<SceneObjectFactory>().FromInstance(_factory).AsSingle();
        Container.Bind<AudioManager>().FromInstance(_audioManager).AsSingle();
        Container.Bind<GameManager>().FromInstance(_gameManager).AsSingle();
    }
}