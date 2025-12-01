using System.Collections;
using System.Collections.Generic;
using UI.QTE;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class TripleTap : MonoBehaviour
    {
        [SerializeField] private List<RadialQte> _radialQtes;
        [SerializeField] private Button _button;
        
        private int _currentRadialQte;
        
        private int _successRadialQte;

        private void OnEnable()
        {
            _button.onClick.AddListener(OnClick);

            foreach (var radial in _radialQtes)
            {
                radial.Successed += OnSucсess;
            }
        }

        private void OnSucсess(QteButtonView obj)
        {
            _successRadialQte++;
            
            Debug.Log(_successRadialQte);
        }

        private void OnDisable()
        {
            _button.onClick.RemoveListener(OnClick);
        }

        private void Start()
        {
            StartCoroutine(PlayCoroutine());
            _currentRadialQte = 0;
        }

        private void OnClick()
        {
            _radialQtes[_currentRadialQte].Check();
            
            _currentRadialQte++;
        }

        public IEnumerator PlayCoroutine()
        {
            for (int i = 0; i < _radialQtes.Count; i++)
            {
                _radialQtes[i].gameObject.SetActive(true);

                yield return new WaitForSeconds(0.7f);
            }
        }
    }
}