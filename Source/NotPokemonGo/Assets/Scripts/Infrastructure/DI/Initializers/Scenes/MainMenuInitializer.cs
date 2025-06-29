using Services.Cameras;
using UI.SpawnPositions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers.Scenes
{
    public class MainMenuInitializer : MonoBehaviour, IInitializable
    {
        private SpawnPositionPresenter _spawnPositionPresenter;
        private ICameraProvider _cameraProvider;

        [Inject]
        public void Construct(SpawnPositionPresenter spawnPositionPresenter, ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
            _spawnPositionPresenter = spawnPositionPresenter;
        }
        
        public void Initialize()
        {
            _cameraProvider.Camera = Camera.main;
            _spawnPositionPresenter.Enable();
        }
    }
}