using Services.Cameras;
using UI.SpawnPositions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers.Scenes
{
    public class MainMenuInitializer : MonoBehaviour, IInitializable
    {
        private ICameraProvider _cameraProvider;

        [Inject]
        public void Construct(ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }
        
        public void Initialize()
        {
            _cameraProvider.Camera = Camera.main;
        }
    }
}