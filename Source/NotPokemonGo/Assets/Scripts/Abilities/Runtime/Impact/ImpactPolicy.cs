using QteSystem.TestQte;

namespace Abilities.Runtime.Impact
{
    public sealed class ImpactPolicy
    {
        public ImpactAction NoQteAction = ImpactAction.ApplyEffectsAndDestroy;

        public ImpactAction OnFail = ImpactAction.ApplyEffectsAndDestroy;
        public ImpactAction OnNormal = ImpactAction.DestroyOnly;
        public ImpactAction OnPerfect = ImpactAction.ReflectToSourceAndDestroy;

        public QteResult DefaultIfMissing = QteResult.Fail;
    }
}