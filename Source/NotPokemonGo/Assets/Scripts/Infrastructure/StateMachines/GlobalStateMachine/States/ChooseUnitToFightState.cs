using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Configs;
using Infrastructure.StateMachines.States.Interfaces;
using LevelSetting;
using Services.SceneServices;
using UI;

namespace Infrastructure.StateMachines.GlobalStateMachine.States
{
    public class ChooseUnitToFightState : IPayloadedState<ChooseUnitToFightPayload>
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;

        private ChooseUnitToFightPanel _chooseUnitToFightPanel;
        private UnitContainerPanel _unitContainerPanel;

        private List<UnitSkinItemViewForChoose> _unitSkinItemViews;
        private LevelConfig _levelConfig;

        public ChooseUnitToFightState(
            IGameStateMachine gameStateMachine, 
            ISceneLoader sceneLoader)
        {
            _gameStateMachine = gameStateMachine;
            _sceneLoader = sceneLoader;
        }

        public void Enter(ChooseUnitToFightPayload payload)
        {
            _chooseUnitToFightPanel = payload.ChooseUnitToFightPanel;
            _levelConfig = payload.Config;

            _unitSkinItemViews = _chooseUnitToFightPanel.UnitSelectionPanelContainer.GetPanels();
            _unitContainerPanel = _chooseUnitToFightPanel.UnitContainerPanel;

            _unitContainerPanel.Show();

            _unitContainerPanel.Clicked += OnUnitContainerPanel;

            _chooseUnitToFightPanel.StartButtonClicked += OnStartButtonClicked;
            _chooseUnitToFightPanel.ExitButtonClicked += OnExitButtonClicked;

            foreach (var unitSkin in _unitSkinItemViews) 
                unitSkin.OnUnitTypeChanged += OnUnitTypeChanged;
        }
        
        public void Exit()
        {
            _chooseUnitToFightPanel.StartButtonClicked -= OnStartButtonClicked;
            _chooseUnitToFightPanel.ExitButtonClicked -= OnExitButtonClicked;
            _unitContainerPanel.Clicked -= OnUnitContainerPanel;

            foreach (var unitSkin in _unitSkinItemViews)
                unitSkin.OnUnitTypeChanged -= OnUnitTypeChanged;
        }

        private void OnUnitContainerPanel(UnitSkinItemView view) 
        {
            view.SetBusy();
            
            SearchFreeView().Initialize(view.UnitItemConfig.Type, view.UnitItemConfig.ContentImage);
        }

        private void OnExitButtonClicked() => 
            _gameStateMachine.Enter<StartScreenState>();

        private void OnStartButtonClicked()
        {
            List<UnitType> unitTypes = new List<UnitType>();

            foreach (var unitSkinItemView in _unitSkinItemViews)
            {
                if (unitSkinItemView.IsFree == false) 
                    unitTypes.Add(unitSkinItemView.UnitType);
            }

            LoadingBattleStatePayload payload = new LoadingBattleStatePayload(unitTypes, _levelConfig);
            
            _sceneLoader.Load(Constants.AssetPath.MainMenuSceneName, () => _gameStateMachine.Enter<LoadingBattleState, LoadingBattleStatePayload>(payload));
        }

        private void OnUnitTypeChanged(UnitType type)
        {
            _unitContainerPanel.Release(type);

            foreach (var skinItem in _unitSkinItemViews)
            {
                if (skinItem.UnitType == type) 
                    skinItem.BeFree();
            }
        }
        
        private UnitSkinItemViewForChoose SearchFreeView() => 
            _unitSkinItemViews.FirstOrDefault(x => x.IsFree);
    }
}