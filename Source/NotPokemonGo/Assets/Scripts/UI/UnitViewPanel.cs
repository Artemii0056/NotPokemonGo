using System;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Services.StaticDataServices;
using Statuses;
using UI.Sliders;
using Units;
using UnityEngine;
using VContainer;

namespace UI
{
    public class UnitViewPanel : MonoBehaviour
    {
        [SerializeField] private List<StatusView> _statusViews;
        [SerializeField] private UnitSliderView _unitSliderView;

        private IStaticDataService _staticDataLoadService;
        private Unit _unit;

        public void Construct(Unit unit)
        {
            _unit = unit;
            _unit.StatusAdded += OnStatusAdded;
            _unit.StatusRemoved += OnStatusRemoved;
            _unit.AgilityChanged += OnAgilityChanged;
            _unit.HealthChanged += OnHealthChanged;
            _unit.Ticked += OnTicked;
        }

        private void OnDestroy()
        {
            _unit.StatusAdded -= OnStatusAdded;
            _unit.StatusRemoved -= OnStatusRemoved;
            _unit.AgilityChanged -= OnAgilityChanged;
            _unit.HealthChanged -= OnHealthChanged;
        }

        [Inject]
        public void Initialize(IStaticDataService staticDataLoadService)
        {
            _staticDataLoadService = staticDataLoadService;
        }
        
        private void OnTicked()
        {
            foreach (StatusView statusView in _statusViews)
            {
                statusView.Tick();
            }
        }

        private void OnHealthChanged(float currentHealth, float maxHealth)
        {
            _unitSliderView.ChangeHealthSlider(currentHealth, maxHealth);
        }

        private void OnAgilityChanged(float currentAgility, float maxAgility)
        {
            _unitSliderView.ChangeAgilitySlider(currentAgility, maxAgility);
        }

        private void OnStatusAdded(Status status)
        {
            StatusView view;

            if (TrySearch(status.Setup.Type, out StatusView statusView) == false)
                view = GetFreeView();
            else
                view = statusView;

            view.Initialize(status, _staticDataLoadService.GetStatusIcon(status.Setup.Type));
        }

        private void OnStatusRemoved(Status status)
        {
            if (TrySearch(status.Setup.Type, out StatusView statusView))
            {
                statusView.Dispose();
            }
        }

        private StatusView GetFreeView()
        {
            return _statusViews.FirstOrDefault(view => !view.HasStatus);
        }

        private bool TrySearch(StatusType searchType, out StatusView status)
        {
            status = null;

            foreach (var searchedStatus in _statusViews)
            {
                if (searchedStatus.HasStatus && searchedStatus.StatusType == searchType)
                {
                    status = searchedStatus;
                    return true;
                }
            }

            return false;
        }
    }
}