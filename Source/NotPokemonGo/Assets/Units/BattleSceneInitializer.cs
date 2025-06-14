using System;
using Characters;
using Characters.Configs.Statuses;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleSceneInitializer : MonoBehaviour
{
    public StatusSetup StatusSetup;
    
    public DamageResolver DamageResolver;
    
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
    
    private EffectManager _effectManager;

    private void OnEnable()
    {
        physicButton.onClick.AddListener(Physical);
        effectButton.onClick.AddListener(Effect);
    }

    private void OnDisable()
    {
        physicButton.onClick.RemoveListener(Physical);
        effectButton.onClick.RemoveListener(Effect);
    }

    private void Start()
    {
        DamageResolver = new DamageResolver();
        _effectManager = new EffectManager();
        
        targetUnit = Instantiate(UnitPrefab);
        targetUnit.Initialize(TargetConfig.Stats, DamageResolver);
        targetUnit.transform.position = TargetSpawnPoint.position;
        
        sourceUnit = Instantiate(UnitPrefab);
        sourceUnit.Initialize(SourceConfig.Stats, DamageResolver);
        sourceUnit.transform.position = SourceSpawnPoint.position;

        UnitStatsPanel.unit = targetUnit;
    }
    
    private void Physical()
    {
        DamageInfo damageInfo = new DamageInfo(DamageType.Physical, 30,sourceUnit);
        
        targetUnit.ReceiveDamage(damageInfo);
    }

    private void Effect()
    {
        HealStatus status = new HealStatus(StatusSetup, targetUnit, sourceUnit, DamageResolver);
        _effectManager.RegisterStatusEffect(status);
    }

    private void Update()
    {
        _effectManager.Update(Time.deltaTime);
        _effectManager.RemoveInactive();
    }
}
