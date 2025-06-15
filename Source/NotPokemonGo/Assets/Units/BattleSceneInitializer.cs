using System;
using Abilities;
using Characters;
using Effects;
using Statuses;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleSceneInitializer : MonoBehaviour
{
    public AbilityView _abilityView;
    public AbilityConfig _abilityConfig;
    
    public StatusSetup StatusSetup;
    
    public EffectResolver EffectResolver;
    
    public UnitStatsPanel UnitStatsPanel;
    
    public Unit UnitPrefab; 
    
    public Transform TargetSpawnPoint;
    public Transform SourceSpawnPoint;
    
    public CharacterStaticData TargetConfig;
    public CharacterStaticData SourceConfig;

    public Unit targetUnit;
    public Unit sourceUnit;
    
    public Button physicButton;
    public Button effectButton;
    public Button abilityButton;
    
    private EffectManager _effectManager;

    private void OnEnable()
    {
        physicButton.onClick.AddListener(Physical);
        effectButton.onClick.AddListener(Effect);
        abilityButton.onClick.AddListener(AbilitySpell);
    }

    private void OnDisable()
    {
        physicButton.onClick.RemoveListener(Physical);
        effectButton.onClick.RemoveListener(Effect);
        abilityButton.onClick.RemoveListener(AbilitySpell);
    }

    private void Start()
    {
        EffectResolver = new EffectResolver();
        _effectManager = new EffectManager();
        
        targetUnit = Instantiate(UnitPrefab);
        targetUnit.Initialize(TargetConfig.Stats, EffectResolver);
        targetUnit.transform.position = TargetSpawnPoint.position;
        
        sourceUnit = Instantiate(UnitPrefab);
        sourceUnit.Initialize(SourceConfig.Stats, EffectResolver);
        sourceUnit.transform.position = SourceSpawnPoint.position;

        UnitStatsPanel.unit = targetUnit;
    }
    
    private void Physical()
    {
        EffectInfo effectInfo = new EffectInfo(EffectType.Damage, 30,sourceUnit);
        
        targetUnit.ReceiveDamage(effectInfo);
    }

    private void Effect()
    {
        HealStatus status = new HealStatus(StatusSetup, targetUnit, sourceUnit, EffectResolver);
        _effectManager.RegisterStatusEffect(status);
    }

    private void AbilitySpell()
    {
        Ability ability = new Ability(_abilityConfig, targetUnit, sourceUnit, EffectResolver);
        
        AbilityView abilityView = Instantiate(_abilityView);
        abilityView._ability = ability;
        abilityView.effectManager = _effectManager;
        abilityView.transform.position = sourceUnit.abilityPos.position;
        abilityView.target = targetUnit;
    }

    private void Update()
    {
        _effectManager.Update(Time.deltaTime);
        _effectManager.RemoveInactive();
    }
}
