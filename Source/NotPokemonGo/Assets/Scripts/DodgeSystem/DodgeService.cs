using DodgeSystem.Configs;
using Services.StaticDataServices;
using Units;

namespace DodgeSystem
{
	public class DodgeService : IDodgeService
	{
		private readonly IStaticDataService _staticDataService;

		public DodgeService(IStaticDataService staticDataService)
		{
			_staticDataService = staticDataService;
		}
		
		public void Dodge(Unit source)
		{
			DodgeConfig dodgeConfig = _staticDataService.GetDodgeConfigByUnitType(source.UnitType);
			
		}
	}
}