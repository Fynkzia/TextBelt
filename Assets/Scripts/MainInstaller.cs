using UnityEngine;
using Zenject;

public class MainInstaller : MonoInstaller
{
    [SerializeField] private DataService dataService;
    public override void InstallBindings()
    {
        Container.Bind<DataService>().FromInstance(dataService).AsSingle();
    }
}