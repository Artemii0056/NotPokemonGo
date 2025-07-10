using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Configs;
using DefaultNamespace;
using UnityEngine;

namespace UI
{
    public class UnitContainerUI : MonoBehaviour //Правая 
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;
        [SerializeField] private List<UnitSkinItemViewForChoose> _unitSkinItemViews = new List<UnitSkinItemViewForChoose>();
        
        private ChooseUnitsForBattle _chooseUnitsForBattle;

        public void Initialize(ChooseUnitsForBattle chooseUnitsForBattle)
        {
            _chooseUnitsForBattle = chooseUnitsForBattle;
        }
        
        public void Show()
        {
            foreach (var itemView in _unitSkinItemViews)
            {
                itemView.gameObject.SetActive(true);
               // itemView.Select();
                itemView.BeFree();
            }
        }
        
        public UnitSkinItemViewForChoose GetFree() => 
            _unitSkinItemViews.FirstOrDefault(x => x.IsFree);

        public  List<UnitType> GetUnitTypes()
        {
            List<UnitType> unitTypes = new List<UnitType>();

            foreach (var unitSkinItemView in _unitSkinItemViews)
            {
                if (unitSkinItemView.IsFree == false)
                {
                    unitTypes.Add(unitSkinItemView.UnitType);
                }
            }

            return unitTypes;
        }

        private void OnEnable()
        {
            foreach (var unitSkin in _unitSkinItemViews)
            {
                unitSkin.OnUnitTypeChanged += OnUnitTypeChanged;
            }
        }
        
        private void OnDisable()
        {
            foreach (var unitSkin in _unitSkinItemViews) 
                unitSkin.OnUnitTypeChanged -= OnUnitTypeChanged;
        }
        
        private void OnUnitTypeChanged(UnitType type)
        {
            _chooseUnitsForBattle.ReleaseButtonByType(type);

            foreach (var skinItem in _unitSkinItemViews)
            {
                if (skinItem.UnitType == type)
                {
                    skinItem.BeFree(); 
                }
            }
        }
    }
}