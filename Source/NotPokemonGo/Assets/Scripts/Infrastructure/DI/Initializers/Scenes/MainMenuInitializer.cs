using UI.SpawnPositions;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Infrastructure.DI.Initializers.Scenes
{
    public class MainMenuInitializer : MonoBehaviour, IInitializable
    {
        private SpawnPositionPresenter _spawnPositionPresenter;

        [Inject]
        public void Construct(SpawnPositionPresenter spawnPositionPresenter)
        {
            _spawnPositionPresenter = spawnPositionPresenter;
        }
        
        public void Initialize()
        {
            Debug.Log("Initializing scenes");
            _spawnPositionPresenter.Enable();
        }

        private void OnDestroy()
        {
            _spawnPositionPresenter.Disable();
        }
    }
}