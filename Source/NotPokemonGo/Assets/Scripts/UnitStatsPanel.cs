using System.Collections.Generic;
using Stats;
using TMPro;
using Units;
using UnityEngine;

public class UnitStatsPanel : MonoBehaviour
{
    [Header("Target Unit")]
    public Unit unit;

    [Header("UI References")]
    public TextMeshProUGUI statsText;

    private void FixedUpdate()
    {
        if (unit == null || statsText == null) 
            return;

        statsText.text = GetStatsText();
    }

    private string GetStatsText()
    {
        var text = "";
        
        foreach (StatType statType in System.Enum.GetValues(typeof(StatType)))
        {
            var a = unit.GetStat(statType);
                text += $"{statType}: {a}\n";
        }

        return text;
    }
}