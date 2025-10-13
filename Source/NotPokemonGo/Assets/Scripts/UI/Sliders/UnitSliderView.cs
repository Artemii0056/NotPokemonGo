using Services;
using Services.Cameras;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;

namespace UI.Sliders
{
    public class UnitSliderView : MonoBehaviour
    {
        [SerializeField] private StatSliderView _agility;
        [SerializeField] private StatSliderView _health;
        
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
            LookTowardCamera(_agility.transform);
        }

        public void ChangeAgilitySlider(float currentValue, float maxValue)
        {
            _agility.ChangeFilling(currentValue, maxValue);
        } 
        
        public void ChangeHealthSlider(float currentValue, float maxValue)
        {
            _health.ChangeFilling(currentValue, maxValue);
        } 
        
        private void LookTowardCamera(Transform source)
        {
            source.LookAt(source.position + _cameraProvider.Camera.transform.rotation * Vector3.forward,
                _cameraProvider.Camera.transform.rotation * Vector3.up);
        }

        /// <summary>
        /// настройка офсетов для объекта. Мб пригодится
        /// </summary>
        private void MoveCloserToCamera()
        {
            Transform parent = transform.parent;

            Vector3 directionToCamera = _cameraProvider.Camera.transform.position - parent.position;
            transform.position = parent.position + directionToCamera.normalized * Offset;
            transform.position += Vector3.up * HeightOffset;
        }
    }
}