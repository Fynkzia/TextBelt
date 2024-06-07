using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private DataService dataService;
    [SerializeField] private GameObject main;
    public override void InstallBindings()
    {
        Container.Bind<DataService>().FromInstance(dataService).AsSingle();
        Container.Bind<GameManager>().FromComponentInHierarchy().AsSingle();
        Container.Bind<UIController>().FromComponentInNewPrefab(main).AsSingle().NonLazy();

        Container.Bind<PhaseMachine>().AsSingle();
        Container.Bind<EventRegistry>().AsSingle();
        Container.Bind<ShowTextAction>().AsSingle();
        Container.Bind<DefaultPhase>().AsSingle();
        Container.Bind<TextMovementPhase>().AsSingle();
        Container.Bind<QuestionPhase>().AsSingle();
        
    }
}