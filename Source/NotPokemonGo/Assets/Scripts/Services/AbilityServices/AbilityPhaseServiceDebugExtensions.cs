using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Services.AbilityServices
{
    public static class AbilityPhaseServiceDebugExtensions
    {
        public static void DumpGateDebugSnapshot(this AbilityPhaseService phaseService, string header)
        {
            if (phaseService == null)
                return;

            try
            {
                var gate = GetPrivateField<PhaseGate>(phaseService, "_finishGate");
                if (gate == null)
                {
                    Debug.LogWarning($"{header} | PhaseGate not found");
                    return;
                }

                string snapshot = BuildGateSnapshot(gate);
                Debug.Log($"{header}\n{snapshot}");
            }
            catch (Exception e)
            {
                Debug.LogWarning($"{header} | Failed to dump PhaseGate snapshot: {e}");
            }
        }

        private static string BuildGateSnapshot(PhaseGate gate)
        {
            // Try to read private fields by reflection: _pending, _holders
            int pending = GetPrivateField<int>(gate, "_pending");
            int version = GetPrivateField<int>(gate, "_version");

            var holdersObj = GetPrivateField<object>(gate, "_holders");

            var lines = new List<string>
            {
                $"PhaseGate version={version} pending={pending}"
            };

            if (holdersObj is IDictionary dict)
            {
                foreach (DictionaryEntry entry in dict)
                    lines.Add($"  token={entry.Key} tag={entry.Value}");
            }
            else
            {
                lines.Add("  holders: <unavailable>");
            }

            return string.Join("\n", lines);
        }

        private static T GetPrivateField<T>(object obj, string fieldName)
        {
            if (obj == null)
                return default;

            var type = obj.GetType();
            var field = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (field == null)
                return default;

            object value = field.GetValue(obj);
            if (value == null)
                return default;

            return value is T t ? t : default;
        }
    }
}
