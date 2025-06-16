using System.Collections.Generic;
using Abilities;
using Characters;
using Effects;
using Factories;
using Statuses;
using Units;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class BattleSceneInitializer : MonoBehaviour
{
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
    public Button armamentAbilityButton;
    public Button castamentAbilityButton;

    private EffectManager _effectManager;

    private StatusFactory _statusFactory;
    private AbilityFactory _abilityFactory;
    private AbilityViewFactory _abilityViewFactory;

    private void OnEnable()
    {
        physicButton.onClick.AddListener(Physical);
        effectButton.onClick.AddListener(Effect);
        abilityButton.onClick.AddListener(AbilitySpell);
        armamentAbilityButton.onClick.AddListener(ArmamentAbilitySpell);
        castamentAbilityButton.onClick.AddListener(CastamentAbilitySpell);
    }

    private void OnDisable()
    {
        physicButton.onClick.RemoveListener(Physical);
        effectButton.onClick.RemoveListener(Effect);
        abilityButton.onClick.RemoveListener(AbilitySpell);
        armamentAbilityButton.onClick.RemoveListener(ArmamentAbilitySpell);
        castamentAbilityButton.onClick.RemoveListener(CastamentAbilitySpell);
    }

    private void Start()
    {
        _statusFactory = new StatusFactory();
        _abilityFactory = new AbilityFactory(_statusFactory);
        EffectResolver = new EffectResolver();
        _effectManager = new EffectManager();
        _abilityViewFactory = new AbilityViewFactory(_effectManager);

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
        EffectInfo effectInfo = new EffectInfo(EffectType.Damage, 30);

        targetUnit.ReceiveDamage(effectInfo);
    }

    private void Effect()
    {
        HealStatus status = new HealStatus(StatusSetup, targetUnit, EffectResolver);
        _effectManager.RegisterStatusEffect(status);
    }

    private void AbilitySpell()
    {
        Ability ability = _abilityFactory.Create(_abilityConfig, targetUnit);

        _abilityViewFactory.Create(sourceUnit.abilityPos.position, ability, _abilityConfig.Prefab, targetUnit);
    }

    private void ArmamentAbilitySpell()
    {
        List<Status> statuses = new List<Status>();
        List<EffectInfo> effects = new List<EffectInfo>();
        
        Ability ability = _abilityFactory.Create(_abilityConfig, targetUnit);

        if (ability.HasArmament)
        {
            foreach (var status in ability.ArmamentSetup.Statuses)
            {
                statuses.Add(_statusFactory.Create(status, targetUnit, EffectResolver));
            }

            foreach (var effectSetup in ability.ArmamentSetup.EffectsSetup)
            {
                effects.Add(new EffectInfo(effectSetup.Type, effectSetup.Value));
            }
        }

        foreach (var status in statuses)
        {
            _effectManager.RegisterStatusEffect(status);
        }

        foreach (var effectInfo in effects)
        {
            targetUnit.ReceiveDamage(effectInfo);
        }

        ParticleSystem effect = Instantiate(_abilityConfig.ParticleSystem);
        effect.transform.position = targetUnit.transform.position;
        effect.Play();
    }
    
    private void CastamentAbilitySpell()
    {
        // Ability ability = _abilityFactory.Create(_abilityConfig, targetUnit, EffectResolver);
        //
        // foreach (var status in ability.Statuses)
        // {
        //     _effectManager.RegisterStatusEffect(status);
        // }
        //
        // ParticleSystem effect = Instantiate(_abilityConfig.ParticleSystem);
        // effect.transform.position = targetUnit.transform.position;
        // effect.Play();
    }

    private void Update()
    {
        _effectManager.Update(Time.deltaTime);
        _effectManager.RemoveInactive();
    }
}