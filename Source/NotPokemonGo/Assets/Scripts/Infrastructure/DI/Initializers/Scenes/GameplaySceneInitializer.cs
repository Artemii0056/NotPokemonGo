using Cinemachine;
using Effects;
using Pools;
using Services.Cameras;
using Services.EffectViewServices;
using Services.StaticDataServices;
using UnityEngine;
using VContainer;

namespace Infrastructure.DI.Initializers.Scenes
{
    public class GameplaySceneInitializer : MonoBehaviour
    {
        [SerializeField] private RectTransform _combatTextCanvas;
        [SerializeField] private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private CombatTextPresenter _combatTextPresenter;
        private ICameraProvider _cameraProvider;
 
    
        [Inject]
        public void Construct(ICameraProvider cameraProvider, IEffectResolver effectResolver, IStaticDataService staticDataService)
        {
            // Debug.Log("Loading CombatText");
        
            _cameraProvider = cameraProvider;
            _cameraProvider.Camera = Camera.main;

            _cameraProvider.VirtualCamera = _cinemachineVirtualCamera;

            CombatTextPool pool = new CombatTextPool(staticDataService.CombatTextPrefab);

            _combatTextPresenter = new CombatTextPresenter(effectResolver, pool, _combatTextCanvas, cameraProvider);
        }
    }
}
