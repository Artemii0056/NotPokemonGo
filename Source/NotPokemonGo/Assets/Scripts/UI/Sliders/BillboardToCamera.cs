using Services.Cameras;
using UnityEngine;
using VContainer;

namespace UI.Sliders
{
    public class BillboardToCamera : MonoBehaviour
    {
        private ICameraProvider _cameraProvider;

        [Inject]
        public void Initialize(ICameraProvider cameraProvider) =>
            _cameraProvider = cameraProvider;

        private void LateUpdate()
        {
            Camera camera = _cameraProvider.Camera;
            
            if (camera == null) 
                return;

            transform.rotation = Quaternion.LookRotation(
                transform.position - camera.transform.position,
                camera.transform.up);
        }
    }
}