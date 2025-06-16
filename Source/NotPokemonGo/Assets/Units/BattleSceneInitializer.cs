using System.Collections;
using System.Collections.Generic;
using Abilities;
using Characters;
using Effects;
using Factories;
using Statuses;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneInitializer : MonoBehaviour
{
    public AbilityConfig _castamentAbilityConfig;
    public AbilityConfig _armamntAbilityConfig;

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
        _abilityViewFactory = new AbilityViewFactory();

        targetUnit = Instantiate(UnitPrefab);
        targetUnit.Initialize(TargetConfig.Stats, EffectResolver);
        targetUnit.transform.position = TargetSpawnPoint.position;

        sourceUnit = Instantiate(UnitPrefab);
        sourceUnit.Initialize(SourceConfig.Stats, EffectResolver);
        sourceUnit.transform.position = SourceSpawnPoint.position;

        UnitStatsPanel.unit = targetUnit;
    }

    #region TestsSpells //Пусть останется в этой ветке для тестов. Или перенесем класс в другую
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
        Ability ability = _abilityFactory.Create(_castamentAbilityConfig, targetUnit);

        _abilityViewFactory.Create(sourceUnit.abilityPos.position, _castamentAbilityConfig.Prefab, targetUnit);
    }
    #endregion

    private void CastamentAbilitySpell()
    {
        List<Status> statuses = new List<Status>();
        List<EffectInfo> effects = new List<EffectInfo>();

        Ability ability = _abilityFactory.Create(_castamentAbilityConfig, targetUnit);

        Debug.Log(ability.ArmamentSetup.EffectsSetup.Count + " " + ability.ArmamentSetup.Statuses.Count);
        
        if (ability.HasCastament)
        {
            foreach (var effectSetup in ability.CastamentSetup.EffectsSetup)
                effects.Add(new EffectInfo(effectSetup.Type, effectSetup.Value));

            foreach (var status in ability.CastamentSetup.Statuses)
                statuses.Add(_statusFactory.Create(status, targetUnit, EffectResolver));
            
            ApplyEffectsToTarget(statuses, effects);
        }

        ParticleSystem effect = Instantiate(_castamentAbilityConfig.ParticleSystem);
        effect.transform.position = targetUnit.transform.position;
        effect.Play();
    }

    private void ArmamentAbilitySpell()
    {
        List<Status> statuses = new List<Status>();
        List<EffectInfo> effects = new List<EffectInfo>();

        Ability ability = _abilityFactory.Create(_armamntAbilityConfig, targetUnit);

        if (ability.HasArmament) 
            CreateStatsAndEffects(ability,  effects,  statuses);
        
        AbilityView abilityView =
            _abilityViewFactory.Create(sourceUnit.abilityPos.position, _armamntAbilityConfig.Prefab, targetUnit);

        StartCoroutine(PlayArmamentAbility(statuses, effects, abilityView));
    }

    private void CreateStatsAndEffects(Ability ability, List<EffectInfo> effects, List<Status> statuses)
    {
        foreach (var effectSetup in ability.ArmamentSetup.EffectsSetup)
            effects.Add(new EffectInfo(effectSetup.Type, effectSetup.Value));

        foreach (var status in ability.ArmamentSetup.Statuses)
            statuses.Add(_statusFactory.Create(status, targetUnit, EffectResolver));
    }

    private IEnumerator PlayArmamentAbility(List<Status> statuses, List<EffectInfo> effects, AbilityView abilityView)
    {
        while (Vector3.Distance(targetUnit.transform.position, abilityView.transform.position) > 0.1f)
            yield return null;

        ApplyEffectsToTarget(statuses, effects);
    }

    private void ApplyEffectsToTarget(List<Status> statuses, List<EffectInfo> effects)
    {
        if (statuses.Count > 0)
        {
            foreach (var status in statuses)
                _effectManager.RegisterStatusEffect(status);
        }

        if (effects.Count > 0)
        {
            foreach (var effectInfo in effects)
                targetUnit.ReceiveDamage(effectInfo);
        }
    }

    private void Update()
    {
        _effectManager.Update(Time.deltaTime);
        _effectManager.RemoveInactive();
    }
}