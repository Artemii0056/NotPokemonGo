using Armaments;
using Stats;

namespace ReactionSystems
{
    public class ReflectFireballReaction : IReaction
    {
        private readonly IArmamentApplicator _armamentApplicator;

        public ReflectFireballReaction(IArmamentApplicator armamentApplicator) =>
            _armamentApplicator = armamentApplicator;

        public bool CanReact(ReactionContext context) => 
            context.Target.GetStat(StatType.DodgeFlag) > 0;

        public void React(ReactionContext context)
        {
            _armamentApplicator.Apply(
                setup: context.Armament.Setup,
                ArmamentFlyingType.Direct,
                source: context.Target,
                targets: context.Source);
        }
    }
}