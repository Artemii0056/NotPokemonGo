using Services.Cameras;
using UnityEngine;
using VContainer;

namespace Infrastructure.DI.Initializers.Scenes
{
    public class MainMenuInitializer : MonoBehaviour
    {
        private ICameraProvider _cameraProvider;

        [Inject]
        public void Construct(ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
            _cameraProvider.Camera = Camera.main;
        }
    }
}