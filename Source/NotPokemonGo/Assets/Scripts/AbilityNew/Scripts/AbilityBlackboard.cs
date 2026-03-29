using System.Collections.Generic;

namespace AbilityNew.Scripts
{
    public class AbilityBlackboard
    {
        private readonly Dictionary<BlackboardKey, object> _data = new();

        public void Set<T>(BlackboardKey key, T value)
        {
            _data[key] = value;
        }

        public bool TryGet<T>(BlackboardKey key, out T value)
        {
            if (_data.TryGetValue(key, out var rawValue) && rawValue is T typedValue)
            {
                value = typedValue;
                return true;
            }

            value = default;
            return false;
        }

        public bool HasKey(BlackboardKey key)
        {
            return _data.ContainsKey(key);
        }

        public void Remove(BlackboardKey key)
        {
            _data.Remove(key);
        }

        public void Clear()
        {
            _data.Clear();
        }
    }
}