using UnityEngine;

namespace QTESystem.TestQTE
{
    public class QTEDifficulty: ScriptableObject
    {
            [Header("Timing")]
            [Tooltip("Время прохода курсора от края до края")]
            public float duration = 1.5f;

            [Header("Zones size (0-1)")]
            [Range(0.01f, 1f)] public float perfectSize = 0.1f;
            [Range(0.01f, 1f)] public float okSize = 0.25f;

            [Header("Behaviour")]
            public bool pingPong = true;
    }
}