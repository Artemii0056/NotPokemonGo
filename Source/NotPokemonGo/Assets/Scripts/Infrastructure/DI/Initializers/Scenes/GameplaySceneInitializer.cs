using Effects;
using Pools;
using Services.Cameras;
using Services.EffectViewServices;
using Services.StaticDataServices;
using UnityEngine;
using VContainer;

public class GameplaySceneInitializer : MonoBehaviour
{
    [SerializeField] private RectTransform _combatTextCanvas;

    private CombatTextPresenter _combatTextPresenter;
    private ICameraProvider _cameraProvider;

    [Inject]
    public void Construct(ICameraProvider cameraProvider, IEffectResolver effectResolver, IStaticDataService staticDataService)
    {
        Debug.Log("Loading CombatText");
        
        _cameraProvider = cameraProvider;
        _cameraProvider.Camera = Camera.main;

        CombatTextPool pool = new CombatTextPool(staticDataService.CombatTextPrefab);

        _combatTextPresenter = new CombatTextPresenter(effectResolver, pool, _combatTextCanvas, Camera.main);
    }
}
