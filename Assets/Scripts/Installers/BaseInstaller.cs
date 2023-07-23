using Buildings;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private DropPoint dropPoint;
        public override void InstallBindings()
        {
            Container.Bind<DropPoint>().FromInstance(dropPoint).AsSingle().NonLazy();
        }
    }
}