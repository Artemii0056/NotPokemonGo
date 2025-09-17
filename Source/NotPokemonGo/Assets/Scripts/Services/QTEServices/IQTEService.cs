using System;
using Abilities;

namespace Services.QTEServices
{
    public interface IQTEService
    {
        void Start(AbilityType abilityType);
        event Action <bool> Completed;
    }
}