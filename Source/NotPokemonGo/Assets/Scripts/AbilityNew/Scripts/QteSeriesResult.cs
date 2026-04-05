using System.Collections.Generic;
using QteSystem.Core;
using QteSystem.Gameplay;

namespace AbilityNew.Scripts
{
    public sealed class QteSeriesResult
    {
        public List<QteResult> Results { get; } = new();

        public bool HasFail => Results.Contains(QteResult.Fail);
        public bool HasNormal => Results.Contains(QteResult.Normal);
        public bool HasPerfect => Results.Contains(QteResult.Perfect);

        public bool AllPerfect => 
            Results.Count > 0 && Results.TrueForAll(x => x == QteResult.Perfect);
        
        public bool AllSuccessOrBetter => Results.Count > 0 && !HasFail;
        
        public void Add(QteResult result) => 
            Results.Add(result);
    }
}