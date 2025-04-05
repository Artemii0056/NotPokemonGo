using UnityEngine;
using UnityEngine.UI;

public class ShopUI : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup _gridLayoutGroup;

    public void AddItem(CharacterSkinItem skinItem)
    {
        skinItem.transform.SetParent(_gridLayoutGroup.transform, false);
        //_gridLayoutGroup.
       // item.GameObject(
        //_gridLayoutGroup.
    }
}