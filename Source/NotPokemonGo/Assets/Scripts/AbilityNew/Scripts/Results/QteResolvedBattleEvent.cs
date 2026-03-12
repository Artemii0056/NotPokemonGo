using QteSystem.TestQte;
using Units;

namespace AbilityNew.Scripts.Results
{
    public class QteResolvedBattleEvent
    {
        public QteResolvedBattleEvent(Unit source, QteResult result)
        {
            Source = source;
            Result = result;
        }

        public Unit Source { get; }
        public QteResult Result { get; }
    }
}