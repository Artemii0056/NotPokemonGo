using TMPro;
using UnityEngine;

public class BattleInfoUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _battleNameText;

    public void SetValue(int value)
    {
        _battleNameText.text = value.ToString();
    }
}