using Armaments;
using DodgeSystem.Configs;
using Services.StaticDataServices;
using Stats;
using Units;
using UnityEngine;

namespace DodgeSystem
{
	public class DodgeService : IDodgeService
	{
		private readonly IStaticDataService _staticDataService;

		public DodgeService(IStaticDataService staticDataService) => 
			_staticDataService = staticDataService;

		public Armament Dodge(Armament armament)
		{
			DodgeConfig dodgeConfig = _staticDataService.GetDodgeConfigByUnitType(armament.Target.UnitType);
			armament.Target.UnitAnimatorController.Play(dodgeConfig.AnimationCashName);

			Armament newArmament = Object.Instantiate(armament);
			newArmament.Initialize(armament.Effects, armament.Statuses, armament.Target, armament.Source);
			Object.Destroy(armament);

			return newArmament;
		}

		public bool CanDodge(Unit unit) =>
			unit.GetStat(StatType.DodgeFlag) > 0;
	}
}