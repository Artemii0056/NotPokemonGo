using System;
using DG.Tweening;
using Services.AbilityServices;

namespace Services
{
    public static class TweenOwnershipExtensions
    {
        public static Tween OwnedByGate(this Tween tween, PhaseGate gate)
        {
            if (tween == null || gate == null)
                return tween;

            if (gate.OwnerId != 0)
                tween.SetId(gate.OwnerId);

            return tween;
        }
    }
}