using System.Collections.Generic;
using Units;

namespace Services.BattleSessionService
{
    public interface IUnitReadyService
    {
        bool HasUnits { get; }
        void SetPlatoons(List<Unit> friends, List<Unit> enemies);
        Unit GiveReadyUnit();
        void Discard();
    }
}