using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
#endif

namespace QteQteTester
{
    [CreateAssetMenu(fileName = "NewGestureTemplate", menuName = "QTE/Gesture Template")]
    public class GestureTemplateSO : ScriptableObject
    {
        public List<Vector2> points = new();
    }
}
