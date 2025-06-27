using System;
using System.Collections.Generic;
using Abilities;
using UnityEngine;

[Serializable]
public class AbilityAnchor
{
    public AbilityType AbilityType;
    public ParticlesType ParticlesType;
    public List<Transform> Transforms;
}