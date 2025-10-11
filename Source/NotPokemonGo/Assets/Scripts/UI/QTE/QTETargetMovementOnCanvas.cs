using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
  public class QTETargetMovementOnCanvas : QTEButtonView
  {
   private const float StartThreshold = 0.05f;       
    private const float CompletionThreshold = 0.98f;  

    [Header("Geometry")]
    [SerializeField] private RectTransform _trackRect;   
    [SerializeField] private RectTransform _handleRect;  

    [Header("Timing")]
    [SerializeField] private float _timer = 5f;          
    [SerializeField] private float _speed = 1f;          

    [Header("Tolerance")]
    [SerializeField, Range(0f, 0.5f)]
    private float _tolerance = 0.05f;                    

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
      EnsureBindings();
    }

    private void OnEnable()
    {
      if (!EnsureBindings())
      {
        enabled = false; 
        return;
      }
      
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
        return;

      TickTimer();
      if (_isCompleted || _isFailed)
        return;

      HandleInput();
    }

    #region Time
    private void TickTimer()
    {
      CurrentTime += Time.deltaTime * _timeScale;

      if (_timeLimit > 0f && CurrentTime >= _timeLimit)
      {
        Fail();
      }
    }

    private float ResolveTimeLimit()
    {
      if (_phasePresenter != null)
      {
        float configured = _phasePresenter.QtePhaseSetup.TargetTime;
        if (configured > 0f) return configured;
      }
      return _timer;
    }

    private float ResolveTimeScale()
    {
      if (_phasePresenter != null)
      {
        float configured = _phasePresenter.QtePhaseSetup.Speed;
        if (configured > 0f) return configured;
      }
      return Mathf.Max(_speed, 0.0001f);
    }
    #endregion

    #region Input
    private void HandleInput()
    {
      if (Input.GetMouseButtonDown(0))
        TryStartDrag(Input.mousePosition);

      if (_isDragging && Input.GetMouseButton(0))
        ContinueDrag(Input.mousePosition);

      if (_isDragging && Input.GetMouseButtonUp(0))
      {
        if (_isCompleted == false)
          Fail();
        _isDragging = false;
      }

      if (Input.touchCount > 0)
      {
        Touch t = Input.GetTouch(0);
        switch (t.phase)
        {
          case TouchPhase.Began:
            TryStartDrag(t.position);
            break;
          case TouchPhase.Moved:
          case TouchPhase.Stationary:
            if (_isDragging) ContinueDrag(t.position);
            break;
          case TouchPhase.Canceled:
          case TouchPhase.Ended:
            if (_isDragging)
            {
              if (_isCompleted == false)
                Fail();
              _isDragging = false;
            }
            break;
        }
      }
    }

    private void TryStartDrag(Vector2 screenPosition)
    {
      if (!ScreenToLocalOnTrack(screenPosition, out Vector2 local)) return;
      if (!IsWithinBounds(local)) return;

      float normalized = LocalToProgress(local.x);
      if (normalized > StartThreshold) 
        return;

      _isDragging = true;
      SetHandleByProgress(0f);
    }

    private void ContinueDrag(Vector2 screenPosition)
    {
      if (!ScreenToLocalOnTrack(screenPosition, out Vector2 local)) return;

      if (!IsWithinBounds(local))
      {
        Fail();
        return;
      }

      Rect rect = _trackRect.rect;
      float clampedX = Mathf.Clamp(local.x, rect.xMin, rect.xMax);

      float newProgress = Mathf.InverseLerp(rect.xMin, rect.xMax, clampedX);
      newProgress = Mathf.Max(GetCurrentProgress(), newProgress);

      SetHandleByProgress(newProgress);

      if (newProgress >= CompletionThreshold)
        Complete();
    }
    #endregion

    #region Geometry helpers
    private bool ScreenToLocalOnTrack(Vector2 screenPosition, out Vector2 local)
    {
      CacheReferences();
      return RectTransformUtility.ScreenPointToLocalPointInRectangle(_trackRect, screenPosition, _uiCamera, out local);
    }

    private float LocalToProgress(float localX)
    {
      Rect rect = _trackRect.rect;
      return Mathf.InverseLerp(rect.xMin, rect.xMax, localX);
    }

    private float GetCurrentProgress()
    {
      Rect rect = _trackRect.rect;
      float x = _handleRect.anchoredPosition.x;
      return Mathf.InverseLerp(rect.xMin, rect.xMax, x);
    }

    private void SetHandleByProgress(float progress)
    {
      Rect rect = _trackRect.rect;
      float x = Mathf.Lerp(rect.xMin, rect.xMax, progress);
      Vector2 position = _handleRect.anchoredPosition;
      position.x = x;
      position.y = 0f;
      _handleRect.anchoredPosition = position;
    }

    private bool IsWithinBounds(Vector2 localPoint)
    {
      Rect rect = _trackRect.rect;

      // Жестко по горизонтали — вне трека сразу фейл
      if (localPoint.x < rect.xMin || localPoint.x > rect.xMax)
        return false;

      // Вертикальный коридор: половина высоты + допуск от высоты
      float halfHeight = rect.height * 0.5f;
      float allowedVertical = halfHeight + rect.height * _tolerance;
      float verticalDistance = Mathf.Abs(localPoint.y);

      return verticalDistance <= allowedVertical;
    }
    #endregion

    #region End states
    private void Complete()
    {
      if (_isCompleted || _isFailed) return;

      _isCompleted = true;
      _isDragging = false;
      SetHandleByProgress(1f);
      Successed?.Invoke(this);
    }

    private void Fail()
    {
      if (_isFailed || _isCompleted) return;

      _isFailed = true;
      _isDragging = false;
      Invalided?.Invoke(this);
    }
    #endregion

    #region Init / Reset
    private void CacheReferences()
    {
      if (_canvas == null)
        _canvas = GetComponentInParent<Canvas>();

      if (_canvas != null && _canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        _uiCamera = _canvas.worldCamera;
      else
        _uiCamera = null;
    }

    private void ResetInternalState()
    {
      CacheReferences();

      if (!EnsureBindings())
      {
        enabled = false; 
        return;
      }

      _timeLimit = ResolveTimeLimit();
      _timeScale = ResolveTimeScale();

      _isDragging = _isCompleted = _isFailed = false;
      CurrentTime = 0f;

      SetHandleByProgress(0f);
    }
    
    private bool EnsureBindings()
    {
      // Если не задан трек – берём RectTransform объекта со скриптом
      if (_trackRect == null)
        _trackRect = GetComponent<RectTransform>();

      // Если не задан хэндл – ищем ребёнка с RectTransform по имени "Handle" или любого первого
      if (_handleRect == null && _trackRect != null)
      {
        // Попробуем по имени
        var t = _trackRect.Find("Handle") as RectTransform;
        if (t != null) _handleRect = t;
        else
        {
          // Любой первый дочерний RectTransform
          for (int i = 0; i < _trackRect.childCount; i++)
          {
            var child = _trackRect.GetChild(i) as RectTransform;
            if (child != null) { _handleRect = child; break; }
          }
        }
      }

      if (_trackRect == null || _handleRect == null)
      {
        Debug.LogError("[QTETargetMovementOnCanvas] Bindings missing. " +
                       "Assign _trackRect and _handleRect in inspector. " +
                       "Hint: Handle should be a child of Track.");
        return false;
      }

      // гарантируем, что хэндл действительно под треком
      if (_handleRect.parent != _trackRect)
      {
        _handleRect.SetParent(_trackRect, worldPositionStays: false);
      }

      return true;
    }

    
    #endregion
  }
}