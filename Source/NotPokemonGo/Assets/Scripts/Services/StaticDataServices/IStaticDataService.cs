using Abilities;
using Characters;
using Characters.Configs;
using Statuses;
using UnityEngine;

namespace Services.StaticDataServices
{
    public interface IStaticDataService
    {
        SpawnPositionConfig GetSpawnPositionConfig(SpawnPositionType spawnPositionType);
        AbilityConfig GetAbilityConfig(AbilityType abilityType);
        Sprite GetStatusIcon(StatusType statusType);
        CharacterConfig GetCharacterConfig(CharacterType characterType);
    }
}