using Stats;
using Units;
using UnityEngine;

namespace UI.Sliders
{
    public class UnitHudView : MonoBehaviour
    {
        [SerializeField] private HealthBarView _healthBar;
        [SerializeField] private AgilityBarView _agilityBar;
        
        private Unit _unit;

        private void OnDisable() => 
            Unbind();
        
        public void Bind(Unit unit)
        {
            Unbind();
            _unit = unit;

            _unit.HealthChanged += OnHealthChanged;
            _unit.AgilityChanged += OnAgilityChanged;

            _agilityBar.Set(_unit.GetStat(StatType.CurrentAgility), _unit.GetStat(StatType.MaxAgility));
        }

        private void Unbind()
        {
            if (_unit == null) 
                return;

            _unit.HealthChanged -= OnHealthChanged;
            _unit.AgilityChanged -= OnAgilityChanged;
            _unit = null;
        }

        private void OnHealthChanged(float current, float max) => 
            _healthBar.Set(current, max);

        private void OnAgilityChanged(float current, float max) => 
            _agilityBar.Set(current, max);

    }
}