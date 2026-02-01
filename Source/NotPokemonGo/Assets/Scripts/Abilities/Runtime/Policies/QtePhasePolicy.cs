using Abilities.Configs;
using QTESystem;

namespace Abilities.Runtime.Policies
{
    public sealed class QtePhasePolicy : AbilityPolicyBase
    {
        private readonly QteBinder _binder;

        public QtePhasePolicy(IQteService qteService) => 
            _binder = new QteBinder(qteService);

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;

            if (phase.QteType == QteType.Unknown)
                return true;

            return _binder.ActiveCount == 0;
        }

        public override void OnAbilityStop(AbilityContext ctx) => 
            _binder.CleanupAll();
    }
}