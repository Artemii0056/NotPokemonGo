using System;
using Services.QTEServices;
using UI.QTE;
using UnityEngine.UI;

namespace QTESystem
{
    [Serializable]
    public class QTEPhaseSetup
    {
        public QTEPhaseType QTEPhaseType;
        public int ClickCount;
        public float Speed;
        public QTEButtonView QTEButtonView;
        public Image Overlay;

        public float TargetTime;
        public float Offset;
        public float TimeToNextTarget;

        // поле которое отвечает за то, через сколько появится следующая QTE
    }
}