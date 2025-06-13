using Characters;
using Units;
using UnityEngine;
using UnityEngine.UI;

public class BattleSceneInitializer : MonoBehaviour
{
    public DamageResolver DamageResolver;
    
    public UnitStatsPanel UnitStatsPanel;
    
    public Unit UnitPrefab; 
    
    public Transform TargetSpawnPoint;
    public Transform SourceSpawnPoint;
    
    public CharacterStaticData TargetConfig;
    public CharacterStaticData SourceConfig;

    public Unit targetUnit;
    public Unit sourceUnit;
    
    public Button Button;

    private void OnEnable()
    {
        Button.onClick.AddListener(Physical);
    }

    private void OnDisable()
    {
        Button.onClick.RemoveListener(Physical);
    }

    private void Start()
    {
        DamageResolver = new DamageResolver();
        
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
}
