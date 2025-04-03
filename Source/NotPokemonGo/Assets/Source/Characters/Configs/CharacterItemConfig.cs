using UnityEngine;
using UnityEngine.UI;

namespace Source.Characters.Configs
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "Characters/CharacterItemConfig")]
    public class CharacterItemConfig : ScriptableObject
    {
        [field: SerializeField] public CharacterConfig CharacterConfig { get; private set; }
        [field: SerializeField] public GameObject CharacterModel { get; private set; }
        [field: SerializeField] public Sprite Image { get; private set; }
        [field: SerializeField] public int Price { get; private set; }
    }
}