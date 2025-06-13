using UnityEngine;

namespace Characters.Configs
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Characters/CharacterItemConfig")]
    public class CharacterItemConfig : ScriptableObject
    {
        [field: SerializeField]
        public CharacterStaticData CharacterStaticData { get; private set; }

        [field: SerializeField] public GameObject CharacterModel { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public Sprite ContentImage { get; private set; }
    }
}