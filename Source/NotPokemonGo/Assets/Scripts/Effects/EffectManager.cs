using System.Collections.Generic;
using Statuses;

namespace Effects
{
    public class EffectManager
    {
       private List<Status> _statusEffects = new List<Status>();

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
            
            foreach (var status in _statusEffects)
            {
                status.UpdateTimer(deltaTime);

                if (status.IsReady)
                    status.Tick();
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