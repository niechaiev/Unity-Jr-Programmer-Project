using UnityEngine;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [SerializeField] private MainManager mainManager;
        public override void InstallBindings()
        {
            Container.Bind<MainManager>().FromComponentInNewPrefab(mainManager).AsSingle().NonLazy();
        }
    }
}