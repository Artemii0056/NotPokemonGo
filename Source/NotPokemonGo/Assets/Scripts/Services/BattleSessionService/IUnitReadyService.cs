using Platoons;
using Units;

namespace Services.BattleSessionService
{
    public interface IUnitReadyService
    {
        bool HasUnits { get; }
        void SetPlatoons(Platoon friends, Platoon enemies);
        Unit GiveReadyUnit();
        void Discard();
    }
}