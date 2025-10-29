using System;
using UnityEngine;
using UnityEngine.UI;

namespace QTESystem.TestQTE
{
    public class SliderReaderManagerOld : MonoBehaviour //Delete?
    {
         [SerializeField] private SliderReader _reader;
        
        [SerializeField] private Slider _slider;

        private void Start()
        {
            _reader.enabled = true;
            //_firstQte.Ended += OnEnded;
        }

        // public void Play()
        // {
        //     _firstQte.enabled = true;
        //     _firstQte.Ended += OnEnded;
        // }

        private void OnEnded(bool state)
        {
            if (state)
            {
                _reader.enabled = false;
               // _secondQte.enabled = true;
                //_secondQte.Ended += OnSecondEnded;
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