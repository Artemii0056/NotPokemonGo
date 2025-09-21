using System;
using UnityEngine;
using UnityEngine.UI;

namespace UI.QTE
{
    public abstract class QTEButtonView : MonoBehaviour
    {
        public abstract event Action<QTEButtonView> Successed;
        public abstract event Action<QTEButtonView> Invalided;
    }
}