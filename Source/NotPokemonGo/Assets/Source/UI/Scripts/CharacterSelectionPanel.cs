using UnityEngine;
using UnityEngine.UI;

namespace Source.UI.Scripts
{
    public class CharacterSelectionPanel : MonoBehaviour
    {
        [SerializeField] private GridLayoutGroup _gridLayoutGroup;

        public void AddItem(CharacterSkinItemView skinItemView) => 
            skinItemView.transform.SetParent(_gridLayoutGroup.transform, false);
    }
}