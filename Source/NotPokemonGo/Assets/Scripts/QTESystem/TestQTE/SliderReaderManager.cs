using System;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class SliderReaderManager : MonoBehaviour
    {
        [SerializeField] private SliderReader _firstQte;
        [SerializeField] private SliderReader _secondQte;
        
        [SerializeField] private Slider _slider;
        
        [SerializeField] private Button _button;

        private void Start()
        {
            _button.onClick.AddListener(Play);
        }

        private void Play()
        {
            _firstQte.enabled = true;
            _firstQte.Ended += OnEnded;
        }

        private void OnEnded(bool state)
        {
            if (state)
            {
                _firstQte.enabled = false;
                _secondQte.enabled = true;
                _secondQte.Ended += OnSecondEnded;
            }
            else
            {
                _slider.gameObject.SetActive(false);
                Debug.Log("Lose");
            }
        }

        private void OnSecondEnded(bool state)
        {
            if (state)
                Debug.Log("Win");
            else
                Debug.Log("Lose");

            _slider.gameObject.SetActive(false);
        }
    }
}