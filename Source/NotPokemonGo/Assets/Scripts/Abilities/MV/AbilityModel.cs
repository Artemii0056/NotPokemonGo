using Abilities.AbilityActions.Armaments;
using Abilities.AbilityActions.Castaments;

namespace Abilities.MV
{
    public class AbilityModel
    {
        public AbilityModel(AbilityConfig config)
        {
            TargetMode = config.TargetMode;
            AbilityType = config.AbilityType;
            
            CastamentSetup = config.CastamentSetup;
            ArmamentSetup = config.ArmamentSetup;

            Cost = config.Cost;
            Cooldown = config.Cooldown;
        }

        public AbilityModel(AbilityModel abilityModel)
        {
            TargetMode = abilityModel.TargetMode;
            AbilityType = abilityModel.AbilityType;
            CastamentSetup = abilityModel.CastamentSetup;
            ArmamentSetup = abilityModel.ArmamentSetup;
            Cost = abilityModel.Cost;
            Cooldown = abilityModel.Cooldown;
        }

        public AbilityType AbilityType { get; private set; }
        public TargetMode TargetMode { get; private set; }
        
        public ArmamentSetup ArmamentSetup { get; private set; }
        public CastamentSetup CastamentSetup { get; private set; }
        
        public bool HasArmament => ArmamentSetup.HasSetupData;
        public bool HasCastament => CastamentSetup.IsActive;

        public float Cost { get; private set; }
        public float CurrentTime { get; private set; } = 0;
        public float Cooldown { get; private set; }
        public bool IsReady => CurrentTime >= Cooldown;
        
        public void UpdateTime(float deltaTime) =>
            CurrentTime += deltaTime;

        public void DiscardCurrentTime() =>
            CurrentTime = 0;
    }
}