using System;
using Characters.Configs;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSkinItemViewForChoose : MonoBehaviour, IPointerClickHandler 
{
    [SerializeField] private UnitSelectionPanel _selectedPanel;
    [SerializeField] private UnitSelectionPanel _unselectedPanel;

    [field: SerializeField] public UnitType UnitType { get; private set; }

    public bool IsFree { get; private set; } = true;

    public event Action<UnitType> OnUnitTypeChanged;

    public void Initialize(UnitType unitType, Sprite sprite)
    {
        IsFree = false;
        BeLock();

        _selectedPanel.SetIcon(sprite);

        UnitType = unitType;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsFree == false)
        {
            OnUnitTypeChanged?.Invoke(UnitType);
            BeFree();
        }
    }

    public void BeLock()
    {
        _selectedPanel.gameObject.SetActive(true);
        _unselectedPanel.gameObject.SetActive(false);
    }

    public void BeFree()
    {
        IsFree = true;
        _selectedPanel.gameObject.SetActive(false);
        _unselectedPanel.gameObject.SetActive(true);
    }
}