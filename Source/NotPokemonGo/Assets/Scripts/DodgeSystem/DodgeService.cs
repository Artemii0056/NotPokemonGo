using Armaments;
using Stats;
using Units;
using UnityEngine;

namespace DodgeSystem
{
	public class DodgeService : IDodgeService
	{
		public Armament Dodge(Armament armament)
		{
			Armament newArmament = Object.Instantiate(armament);
			newArmament.Initialize(armament.Effects, armament.Statuses, armament.Target, armament.Source);
			Object.Destroy(armament);

			return newArmament;
		}

		public bool CanDodge(Unit unit) =>
			unit.GetStat(StatType.DodgeFlag) > 0;
	}
}