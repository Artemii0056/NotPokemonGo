using UnityEngine;

namespace Characters.Configs
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Unit/ItemConfig")]
    public class UnitItemConfig : ScriptableObject
    {
        [field: SerializeField] public UnitConfig UnitConfig { get; private set; }

        [field: SerializeField] public GameObject CharacterModel { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public Sprite ContentImage { get; private set; }
        [field: SerializeField] public UnitType Type { get; private set; }
    }
}