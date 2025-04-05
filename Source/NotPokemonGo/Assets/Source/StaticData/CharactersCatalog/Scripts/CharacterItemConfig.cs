using UnityEngine;
using UnityEngine.UI;

namespace Source.StaticData.CharactersCatalog
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Characters/CharacterItemConfig")]
    public class CharacterItemConfig : ScriptableObject
    {
        [field: SerializeField] public CharacterStaticData.Scripts.CharacterStaticData CharacterStaticData { get; private set; }
        [field: SerializeField] public GameObject CharacterModel { get; private set; }
        [field: SerializeField] public Image Image { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
    }
}