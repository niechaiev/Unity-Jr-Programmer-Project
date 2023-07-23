using UnityEngine;
using Zenject;

namespace Installers
{
    public class BaseInstaller : MonoInstaller
    {
        [SerializeField] private Base dropPoint;
        public override void InstallBindings()
        {
            Container.Bind<Base>().FromInstance(dropPoint).AsSingle().NonLazy();
        }
    }
}