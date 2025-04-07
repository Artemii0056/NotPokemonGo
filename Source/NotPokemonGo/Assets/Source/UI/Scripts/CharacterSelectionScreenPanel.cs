using Source.UI.Scripts;
using UnityEngine;

public class CharacterSelectionScreenPanel : MonoBehaviour
{
    [field: SerializeField] public CharacterPreviewPanel CharacterPreviewPanel { get; private set; }
    [field: SerializeField] public CharacterSelectionPanel CharacterSelectionPanel { get; private set; }
    [field: SerializeField] public CharacterInfoPanel CharacterInfoPanel { get; private set; }
}