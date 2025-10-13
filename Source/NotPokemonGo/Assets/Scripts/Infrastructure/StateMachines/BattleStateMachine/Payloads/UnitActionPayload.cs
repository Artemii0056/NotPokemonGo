using Units;

namespace Infrastructure.StateMachines.BattleStateMachine.Payloads
{
    public class UnitActionPayload
    {
        public readonly Unit UnitSorce;
        public readonly Battlefield Battlefield;
        public UnitActionPayload(Unit unitSorce, Battlefield battlefield)
        {
            UnitSorce = unitSorce;
            Battlefield = battlefield;
        }
    }
}