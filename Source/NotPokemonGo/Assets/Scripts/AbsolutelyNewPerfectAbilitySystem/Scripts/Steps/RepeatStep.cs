using System;
using AbsolutelyNewPerfectAbilitySystem.Configs;

[Serializable]
public class RepeatStep : AbilityStepSO
{
    public int Count;
    public AbilityStepSO Step;
}