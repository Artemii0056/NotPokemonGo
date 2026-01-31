// QteGestureHandler.cs — экспорт шаблона в ScriptableObject + редактор
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "NewGestureTemplate", menuName = "QTE/Gesture Template")]
public class GestureTemplateSO : ScriptableObject
{
    public List<Vector2> points = new();
}
