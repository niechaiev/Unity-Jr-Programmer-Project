using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Installers
{
    public class ProjectInstaller : MonoInstaller
    {
        [FormerlySerializedAs("mainManager")] [SerializeField] private ColorSaver colorSaver;
        public override void InstallBindings()
        {
            Container.Bind<ColorSaver>().FromComponentInNewPrefab(colorSaver).AsSingle().NonLazy();
        }
    }
}