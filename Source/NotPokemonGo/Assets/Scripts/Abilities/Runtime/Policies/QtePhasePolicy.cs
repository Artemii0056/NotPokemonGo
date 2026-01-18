using Abilities.Configs;
using QTESystem;

namespace Abilities.Runtime.Policies
{
    /// <summary>
    /// Rule A для QTE (гейтинг): если в фазе указан QteType != Unknown,
    /// то фазу можно завершить только когда QTE-сессии для этой фазы закрыты.
    ///
    /// ВАЖНО: эта политика НЕ стартует QTE. Старт — в FireballShotsPolicy (BindOnLaunch).
    /// </summary>
    public sealed class QtePhasePolicy : AbilityPolicyBase
    {
        private readonly QteBinder _binder;

        public QtePhasePolicy(IQteService qteService)
        {
            _binder = new QteBinder(qteService);
        }

        public override bool CanFinishPhase(AbilityContext ctx, AbilityPhase phase)
        {
            if (phase == null)
                return true;

            // Если в фазе QTE не требуется — не блокируем.
            if (phase.QteType == QteType.Unknown)
                return true;

            // Если QTE требуется — ждём пока все QTE, стартовавшие "в полёте", закроются.
            return _binder.ActiveCount == 0;
        }

        public override void OnAbilityStop(AbilityContext ctx)
        {
            _binder.CleanupAll();
        }
    }
}