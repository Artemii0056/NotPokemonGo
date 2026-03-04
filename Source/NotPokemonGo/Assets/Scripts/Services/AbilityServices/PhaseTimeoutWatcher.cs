using System;
using Cysharp.Threading.Tasks;
using Units.AnimationControllers;
using UnityEngine;

namespace Services.AbilityServices
{
    /// <summary>
    /// Dev-only helper: waits for phase completion with a timeout.
    /// On timeout it logs PhaseGate holders to help diagnose deadlocks.
    /// </summary>
    public static class PhaseTimeoutWatcher
    {
        /// <summary>
        /// Waits for phase completion, timing out after (animLength + safetySeconds).
        /// Uses unscaled time for the timeout so slowmo won't hide deadlocks.
        /// </summary>
        public static async UniTask<bool> WaitPhaseCompletionWithTimeoutAsync(
            this AbilityPhaseService phaseService,
            AnimatorController animator,
            float safetySeconds,
            float hardCapSeconds,
            string debugLabel,
            System.Threading.CancellationToken token)
        {
            if (phaseService == null)
                return false;

            float animLen = 0f;
            try
            {
                animLen = animator != null ? animator.GetAnimationLength() : 0f;
            }
            catch
            {
                animLen = 0f;
            }

            float timeout = Mathf.Clamp(animLen + Mathf.Max(0.05f, safetySeconds), 0.1f, Mathf.Max(0.1f, hardCapSeconds));

            var completionTask = phaseService.WaitPhaseCompletionAsync(token);
            var timeoutTask = UniTask.Delay(TimeSpan.FromSeconds(timeout), DelayType.UnscaledDeltaTime, PlayerLoopTiming.Update, token);

            int winner = await UniTask.WhenAny(completionTask, timeoutTask);
            if (winner == 0)
                return true;

            // Timeout
            phaseService.DumpGateDebugSnapshot($"PHASE TIMEOUT: {debugLabel} timeout={timeout:0.###}s anim={animLen:0.###}s");
            return false;
        }
    }
}
