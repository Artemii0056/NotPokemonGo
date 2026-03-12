using System.Collections.Generic;

namespace AbilityNew.Scripts.Validation
{
    public sealed class AbilityValidationResult
    {
        private readonly List<string> _errors = new();

        public IReadOnlyList<string> Errors => _errors;
        public bool IsValid => _errors.Count == 0;

        public void AddError(string error)
        {
            if (string.IsNullOrWhiteSpace(error))
                return;

            _errors.Add(error);
        }
    }
}