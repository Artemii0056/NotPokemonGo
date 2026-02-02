using System;
using UnityEngine;

namespace QteSystem.TestQte
{
    [Serializable]
    public class QteDifficulty 
    {
        [Header("Timeline (0–1)")]

        [Range(-0.25f, 1f)]
        public float ActiveStart = 0.15f;

        [Header("OK Zone")]
        [Range(0f, 1f)]
        public float OkStart = 0.35f;
        [Range(0f, 1f)]
        public float OkEnd = 0.45f;

        [Header("Perfect Zone")]
        [Range(0f, 1f)]
        public float PerfectStart = 0.55f;
        [Range(0f, 1f)]
        public float PerfectEnd = 0.60f;

        [Header("Timing")]
        [Min(0.1f)]
        public float duration = 1.5f;
    }
}