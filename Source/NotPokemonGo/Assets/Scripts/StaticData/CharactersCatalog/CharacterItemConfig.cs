using UnityEngine;

namespace Source.StaticData.CharactersCatalog.Scripts
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Characters/CharacterItemConfig")]
    public class CharacterItemConfig : ScriptableObject
    {
        [field: SerializeField]
        public CharacterStaticData.Scripts.CharacterStaticData CharacterStaticData { get; private set; }

        [field: SerializeField] public GameObject CharacterModel { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
        [field: SerializeField] public Sprite ContentImage { get; private set; }
    }
}