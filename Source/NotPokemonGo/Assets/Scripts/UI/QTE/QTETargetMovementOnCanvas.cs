using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
  public class QTETargetMovementOnCanvas : QTEButtonView
  {
    private const float StartThreshold = 0.05f;
    private const float CompletionThreshold = 0.98f;

    [SerializeField] private Slider _slider;
    [SerializeField] private float _timer = 5f;
    [SerializeField] private float _speed = 1f;
    [SerializeField, Range(0f, 0.5f)] private float _tolerance = 0.05f;

    private RectTransform _sliderRect;
    private Canvas _canvas;
    private Camera _uiCamera;
    private QTEPhasePresenter _phasePresenter;

    private float _timeLimit;
    private float _timeScale;
    private bool _isDragging;
    private bool _isCompleted;
    private bool _isFailed;

    public override event Action<QTEButtonView> Successed;
    public override event Action<QTEButtonView> Invalided;

    private void Awake()
    {
      CacheReferences();
    }

    private void OnEnable()
    {
      Debug.Log("asdasdsd");
      ResetInternalState();
    }

    public override void Initialize(QTEPhasePresenter qtePhasePresenter)
    {
      base.Initialize(qtePhasePresenter);
      _phasePresenter = qtePhasePresenter;
      ResetInternalState();
    }

    private void Update()
    {
      if (_isCompleted || _isFailed)
      {
        return;
      }

      TickTimer();

      if (_isCompleted || _isFailed)
      {
        return;
      }

      HandleInput();
    }

    private void TickTimer()
    {
      CurrentTime += Time.deltaTime * _timeScale;

      if (_timeLimit <= 0f || CurrentTime < _timeLimit)
      {
        return;
      }

      Fail();
    }

    private void HandleInput()
    {
      Vector2 mousePosition = Input.mousePosition;

      if (Input.GetMouseButtonDown(0))
      {
        TryStartDrag(mousePosition);
      }

      if (_isDragging && Input.GetMouseButton(0))
      {
        ContinueDrag(mousePosition);
      }

      if (_isDragging && Input.GetMouseButtonUp(0))
      {
        if (_isCompleted == false)
        {
          Fail();
        }

        _isDragging = false;
      }
    }

    private void TryStartDrag(Vector2 screenPosition)
    {
      if (TryGetLocalPoint(screenPosition, out Vector2 localPoint) == false)
      {
        return;
      }

      if (IsWithinBounds(localPoint) == false)
      {
        return;
      }

      float normalized = GetNormalizedProgress(localPoint);

      if (normalized > StartThreshold)
      {
        return;
      }

      _isDragging = true;
      _slider.value = 0f;
    }

    private void ContinueDrag(Vector2 screenPosition)
    {
      if (TryGetLocalPoint(screenPosition, out Vector2 localPoint) == false)
      {
        return;
      }

      if (IsWithinBounds(localPoint) == false)
      {
        Fail();
        return;
      }

      Rect rect = _sliderRect.rect;
      float clampedX = Mathf.Clamp(localPoint.x, rect.xMin, rect.xMax);
      float normalized = Mathf.Max(_slider.value, GetNormalizedProgress(new Vector2(clampedX, localPoint.y)));

      _slider.value = normalized;

      if (normalized >= CompletionThreshold)
      {
        Complete();
      }
    }

    private bool TryGetLocalPoint(Vector2 screenPosition, out Vector2 localPoint)
    {
      CacheReferences();
      return RectTransformUtility.ScreenPointToLocalPointInRectangle(_sliderRect, screenPosition, _uiCamera, out localPoint);
    }

    private float GetNormalizedProgress(Vector2 localPoint)
    {
      Rect rect = _sliderRect.rect;
      return Mathf.InverseLerp(rect.xMin, rect.xMax, localPoint.x);
    }

    private bool IsWithinBounds(Vector2 localPoint)
    {
      Rect rect = _sliderRect.rect;
      float halfHeight = rect.height * 0.5f;
      float allowedVertical = halfHeight + rect.width * _tolerance;
      float verticalDistance = Mathf.Abs(localPoint.y);

      if (verticalDistance > allowedVertical)
      {
        return false;
      }

      float horizontalDistance = 0f;

      if (localPoint.x < rect.xMin)
      {
        horizontalDistance = rect.xMin - localPoint.x;
      }
      else if (localPoint.x > rect.xMax)
      {
        horizontalDistance = localPoint.x - rect.xMax;
      }

      float allowedHorizontal = rect.width * _tolerance;

      return horizontalDistance <= allowedHorizontal;
    }

    private void Complete()
    {
      if (_isCompleted || _isFailed)
      {
        return;
      }

      _isCompleted = true;
      _isDragging = false;
      _slider.value = 1f;
      Successed?.Invoke(this);
    }

    private void Fail()
    {
      if (_isFailed || _isCompleted)
      {
        return;
      }

      _isFailed = true;
      _isDragging = false;
      Invalided?.Invoke(this);
    }

    private void CacheReferences()
    {
      if (_sliderRect == null && _slider != null)
      {
        _sliderRect = _slider.GetComponent<RectTransform>();
      }

      if (_canvas == null)
      {
        _canvas = GetComponentInParent<Canvas>();
      }

      if (_canvas != null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
      {
        _uiCamera = _canvas.worldCamera;
      }
      else
      {
        _uiCamera = null;
      }
    }

    private void ResetInternalState()
    {
      CacheReferences();

      _timeLimit = ResolveTimeLimit();
      _timeScale = ResolveTimeScale();

      _isDragging = false;
      _isCompleted = false;
      _isFailed = false;
      _slider.value = 0f;
      CurrentTime = 0f;
    }

    private float ResolveTimeLimit()
    {
      if (_phasePresenter != null)
      {
        float configured = _phasePresenter.QtePhaseSetup.TargetTime;

        if (configured > 0f)
        {
          return configured;
        }
      }

      return _timer;
    }

    private float ResolveTimeScale()
    {
      if (_phasePresenter != null)
      {
        float configured = _phasePresenter.QtePhaseSetup.Speed;

        if (configured > 0f)
        {
          return configured;
        }
      }

      return Mathf.Max(_speed, 0.0001f);
    }
  }
}