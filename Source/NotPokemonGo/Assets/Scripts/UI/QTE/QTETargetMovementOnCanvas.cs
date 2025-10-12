using System;
using UnityEngine;

namespace UI.QTE
{
  public class QTETargetMovementOnCanvas : QTEButtonView
  {
   private const float StartThreshold = 0.05f;       // старт из первых 5% длины трека
    private const float CompletionThreshold = 0.98f;  // успех при достижении 98%

    [Header("Sprites (world-space)")]
    [SerializeField] private SpriteRenderer _track;   // длинный прямоугольник
    [SerializeField] private SpriteRenderer _handle;  // перетаскиваемый спрайт

    [Header("Camera")]
    [SerializeField] private Camera _camera;          // если пусто, возьмём Camera.main

    [Header("Timing")]
    [SerializeField] private float _timer = 5f;       // лимит (сек), 0 — без лимита
    [SerializeField] private float _speed = 1f;       // множитель таймера

    [Header("Tolerance (relative to track height)")]
    [SerializeField, Range(0f, 0.5f)]
    private float _tolerance = 0.05f;                 // допуск к половине высоты трека

    // внешняя конфигурация через фазу (если есть)
    private QTEPhasePresenter _phasePresenter;

    // вычисляемые параметры
    private float _timeLimit;
    private float _timeScale;
    private bool _isDragging;
    private bool _isCompleted;
    private bool _isFailed;

    private float _baseY;               // линия коридора по Y
    private float _minX, _maxX;         // рабочие границы по X (учитывают размер ручки)
    private float _handleZScreen;       // Z в экранных координатах для корректного ScreenToWorldPoint

    public override event Action<QTEButtonView> Successed;
    public override event Action<QTEButtonView> Invalided;

    private void Awake()
    {
      if (_camera == null) _camera = Camera.main;
      EnsureBindings();
    }

    private void OnEnable()
    {
      if (!EnsureBindings()) { enabled = false; return; }
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
      if (_isCompleted || _isFailed) return;

      TickTimer();
      if (_isCompleted || _isFailed) return;

      HandlePointer();
    }

    // ───────────── Time ─────────────

    private void TickTimer()
    {
      CurrentTime += Time.deltaTime * _timeScale;
      if (_timeLimit > 0f && CurrentTime >= _timeLimit)
        Fail();
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

    // ───────────── Input ─────────────

    private void HandlePointer()
    {
      Vector2 pos;
      if (GetPointerDown(out pos)) TryStartDrag(pos);
      if (_isDragging && GetPointer(out pos)) ContinueDrag(pos);
      if (_isDragging && GetPointerUp(out pos))
      {
        if (!_isCompleted) Fail();
        _isDragging = false;
      }
    }

    private bool GetPointerDown(out Vector2 pos)
    {
      if (Input.GetMouseButtonDown(0)) { pos = Input.mousePosition; return true; }
      if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began) { pos = Input.GetTouch(0).position; return true; }
      pos = default; return false;
    }
    private bool GetPointer(out Vector2 pos)
    {
      if (Input.GetMouseButton(0)) { pos = Input.mousePosition; return true; }
      if (Input.touchCount > 0) { pos = Input.GetTouch(0).position; return true; }
      pos = default; return false;
    }
    private bool GetPointerUp(out Vector2 pos)
    {
      if (Input.GetMouseButtonUp(0)) { pos = Input.mousePosition; return true; }
      if (Input.touchCount > 0)
      {
        var t = Input.GetTouch(0).phase;
        if (t == TouchPhase.Canceled || t == TouchPhase.Ended) { pos = Input.GetTouch(0).position; return true; }
      }
      pos = default; return false;
    }

    // ───────────── Drag logic ─────────────

    private void TryStartDrag(Vector2 screenPos)
    {
      Vector3 world = ScreenToWorldOnTrackPlane(screenPos);
      if (!IsWithinCorridor(world))
        return;

      bool clickedHandle = _handle.bounds.Contains(world);
      float normalized = Mathf.InverseLerp(_minX, _maxX, world.x);

      if (!clickedHandle && normalized > StartThreshold)
        return;

      _isDragging = true;

      float startProgress = Mathf.InverseLerp(_minX, _maxX, Mathf.Clamp(world.x, _minX, _maxX));
      startProgress = Mathf.Max(GetCurrentProgress(), startProgress); // анти-откат
      SetHandleByProgress(startProgress);

      // Debug
      // Debug.Log($"[QTE] Start drag: clickedHandle={clickedHandle}, norm={normalized:F3}, world=({world.x:F2},{world.y:F2},{world.z:F2})");
    }


    private void ContinueDrag(Vector2 screenPos)
    {
      Vector3 world = ScreenToWorldOnTrackPlane(screenPos);

      if (!IsWithinCorridor(world))
      {
        // Debug.Log("[QTE] Out of corridor -> Fail");
        Fail();
        return;
      }

      float clampedX = Mathf.Clamp(world.x, _minX, _maxX);
      float candidate = Mathf.InverseLerp(_minX, _maxX, clampedX);
      float newProgress = Mathf.Max(GetCurrentProgress(), candidate); // анти-откат

      SetHandleByProgress(newProgress);

      if (newProgress >= CompletionThreshold)
        Complete();
    }

    // ───────────── Geometry helpers ─────────────

    private bool EnsureBindings()
    {
      if (_track == null || _handle == null)
      {
        Debug.LogError("[QTE] Assign SpriteRenderers: _track and _handle.");
        return false;
      }
      if (_camera == null)
      {
        _camera = Camera.main;
        if (_camera == null)
        {
          Debug.LogError("[QTE] No Camera assigned and Camera.main is null.");
          return false;
        }
      }
      return true;
    }

    private void RecomputeBounds()
    {
      Bounds tb = _track.bounds;
      Bounds hb = _handle.bounds;

      float handleHalfW = hb.extents.x;

      float trackLeft  = tb.min.x;
      float trackRight = tb.max.x;

      _minX = trackLeft  + handleHalfW;
      _maxX = trackRight - handleHalfW;

      // Базовая линия по Y — центр трека (или оставь hb.center.y, если ручка не по центру)
      _baseY = tb.center.y;
    }

    private Vector3 ScreenToTrackPlane(Vector2 screen)
    {
      // Корректный z, чтобы ScreenToWorldPoint попал в плоскость ручки
      var sp = new Vector3(screen.x, screen.y, _handleZScreen);
      return _camera.ScreenToWorldPoint(sp);
    }

    private bool IsWithinCorridor(Vector3 world)
    {
      if (world.x < _minX || world.x > _maxX) return false;

      // Допуск по вертикали относительно высоты трека
      float halfTrackH = _track.bounds.extents.y;
      float allowed = halfTrackH + _track.bounds.size.y * _tolerance;
      return Mathf.Abs(world.y - _baseY) <= allowed;
    }

    private float GetCurrentProgress()
    {
      float x = _handle.transform.position.x;
      return Mathf.InverseLerp(_minX, _maxX, x);
    }

    private void SetHandleByProgress(float progress01)
    {
      progress01 = Mathf.Clamp01(progress01);
      float x = Mathf.Lerp(_minX, _maxX, progress01);
      var p = _handle.transform.position;
      _handle.transform.position = new Vector3(x, _baseY, p.z);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
      if (_track == null || _handle == null) return;
      RecomputeBounds();
      // Нарисуем коридор
      var b = _track.bounds;
      float halfTrackH = b.extents.y;
      float allowed = halfTrackH + _track.bounds.size.y * _tolerance;

      Gizmos.color = new Color(0,1,0,0.15f);
      Gizmos.DrawCube(new Vector3(b.center.x, _baseY, b.center.z),
        new Vector3(_maxX - _minX + _handle.bounds.size.x, allowed*2f, b.size.z+0.001f));
    }
#endif

    // ───────────── End states ─────────────

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

    // ───────────── Init/Reset ─────────────

    private void ResetInternalState()
    {
      _timeLimit = ResolveTimeLimit();
      _timeScale = ResolveTimeScale();

      _isDragging = _isCompleted = _isFailed = false;
      CurrentTime = 0f;

      RecomputeBounds();
      // Если нужно сбрасывать ручку строго в начало пути:
      // SetHandleByProgress(0f);
      // Иначе оставим где стоит — progress посчитается от актуальной позиции.
    }
    
    private Vector3 ScreenToWorldOnTrackPlane(Vector2 screen)
    {
      // Наша QTE лежит в плоскости XY. Берём плоскость z = zTrack (или zHandle – чаще одинаковы).
      float planeZ = _track.transform.position.z;
      var plane = new Plane(Vector3.forward, new Vector3(0f, 0f, planeZ));

      Ray ray = _camera.ScreenPointToRay(screen);
      if (plane.Raycast(ray, out float enter))
        return ray.GetPoint(enter);

      // На всякий случай — fallback (почти не должен сработать)
      return _camera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, Mathf.Abs(_camera.transform.position.z - planeZ)));
    }
  }
}