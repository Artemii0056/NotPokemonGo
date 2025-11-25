using Units;
using UnityEngine;

namespace Abilities.UnitMovers
{
	public interface IUnitMover
	{
		void MoveByType(UnitMoveType unitMoveType, Unit unit, Vector3 target, float animationLength);
	}
}