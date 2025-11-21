#if UNITY_EDITOR
using System;
using System.Linq;
using Abilities;
using Abilities.AbilitySteps;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions
{
  [CustomPropertyDrawer(typeof(AbilityStepData), true)]
  public class AbilityStepDataDrawer : PropertyDrawer
  {
    private static readonly Type[] StepTypes;
    private static readonly string[] StepTypeNames;

    static AbilityStepDataDrawer()
    {
      StepTypes = TypeCache.GetTypesDerivedFrom<AbilityStepData>()
        .Where(t => !t.IsAbstract && !t.IsGenericType && t.IsClass)
        .ToArray();

      StepTypeNames = StepTypes
        .Select(t => t.Name)
        .Prepend("<None>")
        .ToArray();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
      float height = EditorGUIUtility.singleLineHeight + 4f; 

      if (property.managedReferenceValue == null)
        return height;

      var iterator = property.Copy();
      var end = iterator.GetEndProperty();
      bool enterChildren = true;

      while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
      {
        height += EditorGUI.GetPropertyHeight(iterator, true) + 2f;
        enterChildren = false;
      }

      return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
      var line = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

      Type currentType = property.managedReferenceValue?.GetType();
      int currentIndex = 0; 

      if (currentType != null)
      {
        int idx = Array.IndexOf(StepTypes, currentType);
        if (idx >= 0)
          currentIndex = idx + 1;
      }

      int newIndex = EditorGUI.Popup(line, "Step", currentIndex, StepTypeNames);

      if (newIndex != currentIndex)
      {
        if (newIndex == 0)
        {
          property.managedReferenceValue = null;
        }
        else
        {
          var type = StepTypes[newIndex - 1];
          property.managedReferenceValue = Activator.CreateInstance(type);
          property.isExpanded = true;
        }
      }

      if (property.managedReferenceValue == null)
        return;

      EditorGUI.indentLevel++;
      float y = line.yMax + 2f;

      var iterator = property.Copy();
      var end = iterator.GetEndProperty();
      bool enterChildren = true;

      while (iterator.NextVisible(enterChildren) && !SerializedProperty.EqualContents(iterator, end))
      {
        float h = EditorGUI.GetPropertyHeight(iterator, true);
        var r = new Rect(position.x, y, position.width, h);
        EditorGUI.PropertyField(r, iterator, true);
        y += h + 2f;
        enterChildren = false;
      }

      EditorGUI.indentLevel--;
    }
  }
}
#endif
