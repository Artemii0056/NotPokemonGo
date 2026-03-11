using Abilities.Configs;
using Armaments;
using Armaments.Movers;
using QteSystem.TestQte;

namespace Abilities.Runtime
{
    public sealed class Shot
    {
        public readonly AbilityPhase Phase;
        public readonly ArmamentContext Context;
        public readonly IArmamentMover Mover;

        public bool RequiresQte;
        public QteResult? QteResult;

        public Shot(AbilityPhase phase, ArmamentContext context, IArmamentMover mover)
        {
            Phase = phase;
            Context = context;
            Mover = mover;
            QteResult = QteSystem.TestQte.QteResult.Default;
        }
    }
}