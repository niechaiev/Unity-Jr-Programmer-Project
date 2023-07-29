using Buildings;
using UI;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private DropPoint dropPoint;
        [SerializeField] private UIMainScene uiMainScene;
        
        public override void InstallBindings()
        {
            Container.Bind<DropPoint>().FromInstance(dropPoint).AsSingle().NonLazy();
            Container.Bind<Selector>().AsSingle();
            Container.Bind<UIMainScene>().FromInstance(uiMainScene).AsSingle().NonLazy();
        }
    }
}