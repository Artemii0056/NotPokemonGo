using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace UI
{
    public class UnitSelectionPanelContainer : MonoBehaviour
    {
        [SerializeField] private Transform _gridLayoutGroupTransform;
        [SerializeField] private List<UnitSkinItemViewForChoose> _unitSkinItemViews = new List<UnitSkinItemViewForChoose>();

        public List<UnitSkinItemViewForChoose> GetPanels() => 
            _unitSkinItemViews.ToList();
    }
}