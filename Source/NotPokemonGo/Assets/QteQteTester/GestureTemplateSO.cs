using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
#endif

[CreateAssetMenu(fileName = "NewGestureTemplate", menuName = "QTE/Gesture Template")]
public class GestureTemplateSO : ScriptableObject
{
    public List<Vector2> points = new();
}
