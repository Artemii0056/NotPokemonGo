using Services;
using Services.Cameras;
using UnityEngine;
using VContainer;

namespace UI.Sliders
{
    public class UnitSliderView : MonoBehaviour
    {
        [SerializeField] private AgilitySliderView _agilitySliderView;
        
        private ICameraProvider _cameraProvider;

        [Tooltip("Расстояние на которое нужно приблизиться к камере")]
        public float Offset;

        [Tooltip("Расстояние на которое нужно поднять по высоте")]
        public float HeightOffset;

        [Inject]
        public void Initialize(ICameraProvider cameraProvider)
        {
            _cameraProvider = cameraProvider;
        }
        
        private void LateUpdate()
        {
            if (_cameraProvider == null)
                return;
            
            //MoveCloserToCamera();
            LookTowardCamera(_agilitySliderView.transform);
        }

        private void MoveCloserToCamera()
        {
            Transform parent = transform.parent;

            Vector3 directionToCamera = _cameraProvider.Camera.transform.position - parent.position;
            transform.position = parent.position + directionToCamera.normalized * Offset;
            transform.position += Vector3.up * HeightOffset;
        }

        private void LookTowardCamera(Transform source)
        {
            source.LookAt(source.position + _cameraProvider.Camera.transform.rotation * Vector3.forward,
                _cameraProvider.Camera.transform.rotation * Vector3.up);
        }
    }
}