using UnityEngine;

namespace TimeServices
{
    /// <summary>
    /// Centralized time control service.
    /// Supports temporary time scale overrides (HitStop / SlowMo)
    /// without breaking global time flow.
    /// </summary>
    public sealed class TimeService : ITimeService
    {
        private float _defaultScale = 1f;

        private float _overrideScale = 1f;
        private float _overrideTimer;
        private bool _hasOverride;

        // ===== Public API =====

        public float DeltaTime => Time.deltaTime;
        public float FixedDeltaTime => Time.fixedDeltaTime;
        public float UnscaledDeltaTime => Time.unscaledDeltaTime;

        public float HalfTime => Time.deltaTime * 0.5f;
        public float QuarterTime => Time.deltaTime * 0.25f;

        public void HitStop(float timeScale, float duration)
        {
            ApplyOverride(
                Mathf.Clamp(timeScale, 0.01f, 1f),
                Mathf.Max(0.01f, duration)
            );
        }

        public void SlowMo(float timeScale, float duration)
        {
            ApplyOverride(
                Mathf.Clamp(timeScale, 0.05f, 1f),
                Mathf.Max(0.01f, duration)
            );
        }

        // ===== Internal Logic =====

        private void ApplyOverride(float scale, float duration)
        {
            // Always override previous effect (last call wins)
            _overrideScale = scale;
            _overrideTimer = duration;

            if (!_hasOverride)
            {
                _defaultScale = Time.timeScale;
                _hasOverride = true;
            }

            Time.timeScale = _overrideScale;
        }

        /// <summary>
        /// Must be called every frame (e.g. from a MonoBehaviour).
        /// </summary>
        public void Tick()
        {
            if (!_hasOverride)
                return;

            _overrideTimer -= Time.unscaledDeltaTime;

            if (_overrideTimer <= 0f)
            {
                Time.timeScale = _defaultScale;
                _hasOverride = false;
            }
        }
    }
}
