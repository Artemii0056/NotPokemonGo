using AbilityNew.AbilityDefinition;
using AbilityNew.Scripts.AbilityExecutor;
using AbilityNew.Scripts.Steps.Presentation;
using Cysharp.Threading.Tasks;
using Services.Cameras;

namespace AbilityNew.Scripts.Executors.Presentation
{
    public class CameraShakeExecutor : AbilityStepExecutor<CameraShakeStep>
    {
        private ICameraShakeService _cameraService;
        
        public CameraShakeExecutor(ICameraShakeService cameraService) => 
            _cameraService = cameraService;

        public override UniTask Execute(CameraShakeStep step, AbilityExecutionRuntime runtime)
        {
            throw new System.NotImplementedException();
        }
    }
}