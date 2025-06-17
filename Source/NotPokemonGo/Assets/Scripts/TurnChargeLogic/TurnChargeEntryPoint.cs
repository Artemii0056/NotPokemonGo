using System.Collections.Generic;
using Characters;
using Services;
using UnityEngine;

namespace TurnChargeLogic
{
    public class TurnChargeEntryPoint : MonoBehaviour, ICoroutineRunner
    {
        // [SerializeField] private List<Character> _characters1 = new List<Character>();
        // [SerializeField] private List<Character> _characters2 = new List<Character>();
        //
        // [Header("Time for ability platton1")]
        // [SerializeField] private float minTimeForFirstPlaton;
        // [SerializeField] private float maxTimeForFirstPlaton;
        //
        // [Header("Time for ability platton2")]
        // [SerializeField] private float minTimeForSecondPlaton;
        // [SerializeField] private float maxTimeForSecondPlaton;
        // private Battlefield _battlefield;
        //
        // public CharacterStaticData TargetConfig;
        // public CharacterStaticData SourceConfig;
        //
        // private void Start()
        // {
        //     foreach (Character character in _characters1)
        //     {
        //         character.Name = "Lox";
        //         character.gameObject.name = "Lox";
        //         character.Initialize(TargetConfig.Stats);
        //     }
        //
        //     foreach (Character character in _characters2)
        //     {
        //         character.Name = "Gay";
        //         character.gameObject.name = "Gay";
        //         character.Initialize(SourceConfig.Stats);
        //     }
        //     
        //     Platoon platoon1 = new Platoon(_characters1, this, minTimeForFirstPlaton, maxTimeForFirstPlaton);
        //     Platoon platoon2 = new Platoon(_characters2, this, minTimeForSecondPlaton, maxTimeForSecondPlaton);
        //     
        //     _battlefield = new Battlefield(platoon1, platoon2);
        // }
        //
        // private void Update()
        // {
        //     _battlefield.Tick(Time.deltaTime);
        // }
    }
}