using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private DataService dataService;
    [SerializeField] private GameObject main;
    [SerializeField] private GameObject gameManager;
    public override void InstallBindings()
    {
        Container.Bind<DataService>().FromInstance(dataService).AsSingle();
        //Container.Instantiate<GameManager>();
        Container.Bind<UIController>().FromComponentInNewPrefab(main).AsSingle().NonLazy();
        
    }
}