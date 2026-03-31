using Units;

namespace AbilityNew.Scripts.Results
{
    public class CounterAttackRequest
    {
        public CounterAttackRequest(Unit reactor, Unit target)
        {
            Reactor = reactor;
            Target = target;
        }

        public Unit Reactor { get; }
        public Unit Target { get; }
    }
}