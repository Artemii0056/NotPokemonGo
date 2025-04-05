using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionPanel : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    public void AddItem(CharacterSkinItem skinItem) => 
        skinItem.transform.SetParent(_gridLayoutGroup.transform, false);
}