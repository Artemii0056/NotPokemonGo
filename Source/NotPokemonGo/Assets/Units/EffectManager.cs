using System.Collections.Generic;
using Characters.Configs.Statuses;

namespace Units
{
    public class EffectManager
    {
        List<Status> _statusEffects = new List<Status>();

        public void RegisterStatusEffect(Status status)
        {
            _statusEffects.Add(status);
            status.OnApply();
        }

        public void UnregisterStatusEffect(Status status)
        {
            _statusEffects.Remove(status);
        }

        public void Update(float deltaTime)
        {
            if (_statusEffects.Count <= 0)
                return;
            
            foreach (var effect in _statusEffects)
            {
                effect.UpdateTimer(deltaTime);

                if (effect.IsReady)
                    effect.Tick();
            }
        }

        public void RemoveInactive()
        {
            // if (_statusEffects.Count <= 0)
            //     return;
            
            for (int i = _statusEffects.Count - 1; i >= 0; i--)
            {
                if (_statusEffects[i].TickCount <= 0)
                {
                    UnregisterStatusEffect(_statusEffects[i]);
                }
            }
        }
    }
}