using DG.Tweening;

namespace Services.AbilityServices.Executors
{
    public static class PhaseGateTweenExtensions
    {
        public static Tween WithPhaseGate(this Tween tween, PhaseGate gate, string tag)
        {
            if (tween == null || gate == null)
                return tween;

            var token = gate.Acquire(tag);
            bool disposed = false;

            void Release()
            {
                if (disposed) return;
                disposed = true;
                token.Dispose();
            }

            if (gate.OwnerId != 0)
                tween.SetId(gate.OwnerId);

            tween.OnKill(Release);
            tween.OnComplete(Release);

            return tween;
        }

        public static Sequence WithPhaseGate(this Sequence seq, PhaseGate gate, string tag)
        {
            if (seq == null || gate == null)
                return seq;

            var token = gate.Acquire(tag);
            bool disposed = false;

            void Release()
            {
                if (disposed) return;
                disposed = true;
                token.Dispose();
            }

            if (gate.OwnerId != 0)
                seq.SetId(gate.OwnerId);

            seq.OnKill(Release);
            seq.OnComplete(Release);

            return seq;
        }
    }
}
