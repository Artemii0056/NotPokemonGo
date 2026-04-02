using System;
using System.Collections.Generic;

namespace AbilityNew.Scripts.Presentation
{
    [Serializable]
    public sealed class PresentationEntry
    {
        public AbilityPresentationSignal Signal;
        public List<PresentationCase> Cases = new();
    }
}