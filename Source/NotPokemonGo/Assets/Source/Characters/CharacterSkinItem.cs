using UnityEngine;
using UnityEngine.UI;

public class CharacterSkinItem : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void InitImage(Sprite sprite)
    {
        _image.sprite = sprite;
    }
}
