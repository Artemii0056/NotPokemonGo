using System;
using System.Collections.Generic;
using Abilities;
using QTESystem;
using Services.InputServices;
using Services.RaycastServices;
using Units;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UI.QTE
{
    public class QTEPhasePresenter
    {
        private bool _isProceeded;

        public QTEPhasePresenter(QTEPhaseSetup qtePhaseSetup)
        {
            _isProceeded = true;

            QTESpawer qteSpawer = new QTESpawer();

            switch (qtePhaseSetup.QTEPhaseType)
            {
                case QTEPhaseType.ТапатьПоВрагу:
                    QTEButtonView qteRaycasterView = qteSpawer.Spawn(qtePhaseSetup.QTEButtonView);
                    qteRaycasterView.Successed += OnSuccessed;
                    qteRaycasterView.Invalided += OnInvalided;
                    break;

                case QTEPhaseType.ТапатьПоUI:
                    QTEButtonView qteButtonView = qteSpawer.Spawn(qtePhaseSetup.QTEButtonView);
                    qteButtonView.Successed += OnSuccessed;
                    qteButtonView.Invalided += OnInvalided;
                    break;

                case QTEPhaseType.ПереместитьЦельПоКанвасу:
                    // заспавнить UI
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public bool IsSuccess { get; private set; }

        public bool IsProceeded() =>
            _isProceeded;

        private void OnInvalided(QTEButtonView qteButtonView)
        {
            qteButtonView.Invalided -= OnInvalided;
            _isProceeded = false;
            IsSuccess = false;
        }

        private void OnSuccessed(QTEButtonView qteButtonView)
        {
            qteButtonView.Successed -= OnSuccessed;
            _isProceeded = false;
            IsSuccess = true;
        }
    }

    public class QTERaycasterView : QTEButtonView
    {
        private IRaycastService _raycastService;
        private ITargetSelector _targetSelector;
        private IInputReader _inputReader;

        public override event Action<QTEButtonView> Successed;
        public override event Action<QTEButtonView> Invalided;

        [Inject]
        public void Construct(IInputReader inputReader, IRaycastService raycastService, ITargetSelector targetSelector)
        {
            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
            _inputReader = inputReader;
            _targetSelector = targetSelector;
            _raycastService = raycastService;
        }
        
        private void OnLeftMouseButtonClicked()
        {
            if (_raycastService.Raycast(out Unit unit) == false) 
                Invalided?.Invoke(this);
            
            List<Unit> units = _targetSelector.GetTargets(TargetMode.Single);

            foreach (Unit unitInList in units)
            {
                if (unitInList == unit)
                {
                    Successed?.Invoke(this);
                    break;
                }
            }
        }

        private void OnDestroy() => 
            _inputReader.LeftMouseButtonPressed += OnLeftMouseButtonClicked;
    }

    public class QTETapUIView : QTEButtonView
    {
        public RectTransform rectTransform;
        
        public float TargetTime;
        public float Offset;
        public Button Button;
        public Image TargetImage;

        public Image Halo;
        public Image End;

        public float CurrentTime;

        public float Speed;

        private Vector2 _initialSize;
        private Vector2 _endSize;
        private Vector2 _targetSize;

        private Vector2 _visualTargetSize;
        private Vector2 _visualEndSize;
        
        public override event Action<QTEButtonView> Successed;
        public override event Action<QTEButtonView> Invalided;

        
        private bool _isSuccesTime => CurrentTime >= TargetTime - Offset && CurrentTime <= TargetTime;


        private void OnEnable() => 
            Button.onClick.AddListener(Clicked);

        private void OnDisable() => 
            Button.onClick.RemoveListener(Clicked);

        public void Initialize(float offset, float targetTime, Vector2 position)
        {
            rectTransform.anchoredPosition = position;
            
            TargetTime = targetTime;
            Offset = offset;
            
            _targetSize = TargetImage.rectTransform.sizeDelta;
            _endSize = End.rectTransform.sizeDelta;
            
            // Halo должен достичь targetSize в момент (TargetTime - Offset)
            float progressDuration = TargetTime - Offset;
            float scaleMultiplier = 1f + Offset / progressDuration;
            _initialSize = _targetSize * scaleMultiplier;

            Halo.rectTransform.sizeDelta = _initialSize;
            CurrentTime = 0;
        }

        private void Update()
        {
            CurrentTime += Time.deltaTime * Speed;

            if (_isSuccesTime)
            {
                TargetImage.color = Color.green;
            }
            else
            {
                TargetImage.color = Color.red;
            }

            if (CurrentTime <= TargetTime - Offset)
            {
                // Фаза 1: от начального до targetSize
                float progress = Mathf.Clamp01(CurrentTime / (TargetTime - Offset));
                Halo.rectTransform.sizeDelta = Vector2.Lerp(_initialSize, _targetSize, progress);
            }
            else if (CurrentTime <= TargetTime)
            {
                // Фаза 2: от targetSize до endSize
                float progress = Mathf.Clamp01((CurrentTime - (TargetTime - Offset)) / Offset);
                Halo.rectTransform.sizeDelta = Vector2.Lerp(_targetSize, _endSize, progress);
            }
            else
            {
                // После TargetTime — остаётся маленьким
                Halo.rectTransform.sizeDelta = _endSize;
            }
        }

        private void Clicked()
        {
            if (_isSuccesTime) 
                Successed?.Invoke(this);
            else
                Invalided?.Invoke(this);
        }

        public void Reset()
        {
            CurrentTime = 0;
            rectTransform.anchoredPosition = Vector2.zero;
        }
    }
}