using Units;

namespace Battlefields
{
    public interface IBattleUnitContainer
    {
        void Add(Unit unit);
        Unit Give();
        void Reset();
    }
}