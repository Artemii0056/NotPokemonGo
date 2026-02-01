#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using Abilities.Configs;

//[CustomPropertyDrawer(typeof(PhaseSignalAction))]
public sealed class PhaseSignalActionDrawer : PropertyDrawer
{
    private enum ActionType { Particles, Move, Camera, Sound, Time, Armament, Castament }

    private static readonly Dictionary<string, HashSet<ActionType>> State =
        new Dictionary<string, HashSet<ActionType>>();

    private const float Spacing = 4f;
    private const float BtnW = 104f;
    private const float RemoveW = 22f;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var key = property.propertyPath;
        if (!State.TryGetValue(key, out var actions))
        {
            actions = new HashSet<ActionType>();
            State[key] = actions;
        }

        RestoreSectionsFromData(property, actions);

        position.height = EditorGUIUtility.singleLineHeight;
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
        position.y += position.height + Spacing;

        if (!property.isExpanded)
            return;

        EditorGUI.indentLevel++;

        DrawField(ref position, property, "Signal");
        DrawField(ref position, property, "TargetMode");

        DrawActionsHeader(ref position, () => ShowAddMenu(property, actions));

        if (actions.Contains(ActionType.Particles))
            DrawParticles(ref position, property, actions);

        if (actions.Contains(ActionType.Move))
            DrawMove(ref position, property, actions);

        if (actions.Contains(ActionType.Camera))
            DrawCamera(ref position, property, actions);

        if (actions.Contains(ActionType.Sound))
            DrawSound(ref position, property, actions);

        if (actions.Contains(ActionType.Time))
            DrawTime(ref position, property, actions);

        if (actions.Contains(ActionType.Armament))
            DrawSingle(ref position, property, actions, ActionType.Armament, "Armament", "ArmamentSetup");

        if (actions.Contains(ActionType.Castament))
            DrawSingle(ref position, property, actions, ActionType.Castament, "Castament", "CastamentSetup");

        EditorGUI.indentLevel--;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float h = EditorGUIUtility.singleLineHeight; // foldout

        if (!property.isExpanded)
            return h;

        var key = property.propertyPath;
        if (!State.TryGetValue(key, out var actions))
        {
            actions = new HashSet<ActionType>();
            State[key] = actions;
        }

        // Auto-restore visible sections based on serialized data (survives Unity restart)
        RestoreSectionsFromData(property, actions);

        h += Spacing;

        h += GetFieldHeight(property, "Signal");
        h += GetFieldHeight(property, "TargetMode");

        h += EditorGUIUtility.singleLineHeight + Spacing; // Actions header

        if (actions.Contains(ActionType.Particles))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "ParticlePrefab");
            h += GetFieldHeight(property, "ParticleSpawnType");
            h += GetFieldHeight(property, "ParticleOwner");
        }

        if (actions.Contains(ActionType.Move))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "MoveCommand");
            h += GetFieldHeight(property, "MoveMode");
            h += GetFieldHeight(property, "CustomPoint");
            h += GetFieldHeight(property, "MoveDuration");
            h += GetFieldHeight(property, "MoveDelay");
            h += GetFieldHeight(property, "StopDistance");
            h += GetFieldHeight(property, "JumpPower");
            h += GetFieldHeight(property, "NumJumps");
        }

        if (actions.Contains(ActionType.Camera))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "CameraCommand");
            h += GetFieldHeight(property, "CameraBlendTimeout");
        }

        if (actions.Contains(ActionType.Sound))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "SfxClip");
            h += GetFieldHeight(property, "SfxVolume");
            h += GetFieldHeight(property, "Sfx2D");
        }

        if (actions.Contains(ActionType.Time))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "TimeEffect");
            h += GetFieldHeight(property, "TimeScale");
            h += GetFieldHeight(property, "TimeDuration");
        }

        if (actions.Contains(ActionType.Armament))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "ArmamentSetup");
        }

        if (actions.Contains(ActionType.Castament))
        {
            h += SectionHeaderHeight();
            h += GetFieldHeight(property, "CastamentSetup");
        }

        return h + 2f;
    }

    // ---------------- Auto-restore ----------------

    private static void RestoreSectionsFromData(SerializedProperty root, HashSet<ActionType> actions)
    {
        // Particles: show if any particle-related data is set
        var particlePrefab = root.FindPropertyRelative("ParticlePrefab");
        if (particlePrefab != null && particlePrefab.objectReferenceValue != null)
            actions.Add(ActionType.Particles);

        // Move: show if MoveCommand != None OR any move numeric/custom point differs from defaults
        var moveCommand = root.FindPropertyRelative("MoveCommand");
        if (moveCommand != null && IsEnumNotNone(moveCommand))
            actions.Add(ActionType.Move);

        // Camera: show if CameraCommand != None
        var camCommand = root.FindPropertyRelative("CameraCommand");
        if (camCommand != null && IsEnumNotNone(camCommand))
            actions.Add(ActionType.Camera);

        // Sound: show if clip assigned
        var sfxClip = root.FindPropertyRelative("SfxClip");
        if (sfxClip != null && sfxClip.objectReferenceValue != null)
            actions.Add(ActionType.Sound);

        // Time: show if TimeEffect != None
        var timeEffect = root.FindPropertyRelative("TimeEffect");
        if (timeEffect != null && IsEnumNotNone(timeEffect))
            actions.Add(ActionType.Time);

        // Armament/Castament: show if setup assigned
        var arm = root.FindPropertyRelative("ArmamentSetup");
        if (arm != null && arm.objectReferenceValue != null)
            actions.Add(ActionType.Armament);

        var cast = root.FindPropertyRelative("CastamentSetup");
        if (cast != null && cast.objectReferenceValue != null)
            actions.Add(ActionType.Castament);
    }

    private static bool IsEnumNotNone(SerializedProperty enumProp)
    {
        // We compare by name so reordering enum values won't break the check.
        if (enumProp.propertyType != SerializedPropertyType.Enum)
            return false;

        int idx = enumProp.enumValueIndex;
        if (idx < 0 || idx >= enumProp.enumNames.Length)
            return false;

        return enumProp.enumNames[idx] != "None";
    }

    // ---------------- Refresh ----------------

    private static void RequestInspectorRefresh(SerializedProperty anyProp)
    {
        EditorApplication.delayCall += () =>
        {
            if (anyProp != null && anyProp.serializedObject != null)
                EditorUtility.SetDirty(anyProp.serializedObject.targetObject);

            InternalEditorUtility.RepaintAllViews();
            EditorApplication.QueuePlayerLoopUpdate();
        };
    }

    // ---------------- Drawing helpers ----------------

    private static float SectionHeaderHeight() => EditorGUIUtility.singleLineHeight + Spacing;

    private static float GetFieldHeight(SerializedProperty root, string name)
    {
        var p = root.FindPropertyRelative(name);
        if (p == null) return 0f;
        return EditorGUI.GetPropertyHeight(p, true) + Spacing;
    }

    private static void DrawField(ref Rect pos, SerializedProperty root, string name)
    {
        var p = root.FindPropertyRelative(name);
        if (p == null) return;

        float h = EditorGUI.GetPropertyHeight(p, true);
        pos.height = h;
        EditorGUI.PropertyField(pos, p, true);
        pos.y += h + Spacing;
        pos.height = EditorGUIUtility.singleLineHeight;
    }

    private static void DrawActionsHeader(ref Rect pos, System.Action onAdd)
    {
        var line = pos;
        line.height = EditorGUIUtility.singleLineHeight;

        var labelRect = line;
        labelRect.width -= (BtnW + 6f);

        var buttonRect = line;
        buttonRect.x = labelRect.xMax + 6f;
        buttonRect.width = BtnW;

        EditorGUI.LabelField(labelRect, "Actions", EditorStyles.boldLabel);
        if (GUI.Button(buttonRect, "Add Action"))
            onAdd?.Invoke();

        pos.y += line.height + Spacing;
    }

    private static void DrawSectionHeader(ref Rect pos, string title, System.Action onRemove)
    {
        var line = pos;
        line.height = EditorGUIUtility.singleLineHeight;

        var labelRect = line;
        labelRect.width -= (RemoveW + 4f);

        var removeRect = line;
        removeRect.x = labelRect.xMax + 4f;
        removeRect.width = RemoveW;

        EditorGUI.LabelField(labelRect, title, EditorStyles.helpBox);
        if (GUI.Button(removeRect, "✕"))
            onRemove?.Invoke();

        pos.y += line.height + Spacing;
    }

    // ---------------- Sections ----------------

    private static void DrawParticles(ref Rect pos, SerializedProperty root, HashSet<ActionType> actions)
    {
        DrawSectionHeader(ref pos, "Particles", () =>
        {
            actions.Remove(ActionType.Particles);
            // keep data? we clear prefab only to avoid auto-restore bringing it back
            var p = root.FindPropertyRelative("ParticlePrefab");
            if (p != null) p.objectReferenceValue = null;
            RequestInspectorRefresh(root);
        });

        DrawField(ref pos, root, "ParticlePrefab");
        DrawField(ref pos, root, "ParticleSpawnType");
        DrawField(ref pos, root, "ParticleOwner");
    }

    private static void DrawMove(ref Rect pos, SerializedProperty root, HashSet<ActionType> actions)
    {
        DrawSectionHeader(ref pos, "Movement", () =>
        {
            actions.Remove(ActionType.Move);
            var cmd = root.FindPropertyRelative("MoveCommand");
            if (cmd != null && cmd.propertyType == SerializedPropertyType.Enum)
            {
                // set to None if exists, else 0
                int none = System.Array.IndexOf(cmd.enumNames, "None");
                cmd.enumValueIndex = none >= 0 ? none : 0;
            }
            RequestInspectorRefresh(root);
        });

        DrawField(ref pos, root, "MoveCommand");
        DrawField(ref pos, root, "MoveMode");
        DrawField(ref pos, root, "CustomPoint");
        DrawField(ref pos, root, "MoveDuration");
        DrawField(ref pos, root, "MoveDelay");
        DrawField(ref pos, root, "StopDistance");
        DrawField(ref pos, root, "JumpPower");
        DrawField(ref pos, root, "NumJumps");
    }

    private static void DrawCamera(ref Rect pos, SerializedProperty root, HashSet<ActionType> actions)
    {
        DrawSectionHeader(ref pos, "Camera", () =>
        {
            actions.Remove(ActionType.Camera);
            var cmd = root.FindPropertyRelative("CameraCommand");
            if (cmd != null && cmd.propertyType == SerializedPropertyType.Enum)
            {
                int none = System.Array.IndexOf(cmd.enumNames, "None");
                cmd.enumValueIndex = none >= 0 ? none : 0;
            }
            RequestInspectorRefresh(root);
        });

        DrawField(ref pos, root, "CameraCommand");
        DrawField(ref pos, root, "CameraBlendTimeout");
    }

    private static void DrawSound(ref Rect pos, SerializedProperty root, HashSet<ActionType> actions)
    {
        DrawSectionHeader(ref pos, "Sound", () =>
        {
            actions.Remove(ActionType.Sound);
            var clip = root.FindPropertyRelative("SfxClip");
            if (clip != null) clip.objectReferenceValue = null;
            RequestInspectorRefresh(root);
        });

        DrawField(ref pos, root, "SfxClip");
        DrawField(ref pos, root, "SfxVolume");
        DrawField(ref pos, root, "Sfx2D");
    }

    private static void DrawTime(ref Rect pos, SerializedProperty root, HashSet<ActionType> actions)
    {
        DrawSectionHeader(ref pos, "Time Effect", () =>
        {
            actions.Remove(ActionType.Time);
            var eff = root.FindPropertyRelative("TimeEffect");
            if (eff != null && eff.propertyType == SerializedPropertyType.Enum)
            {
                int none = System.Array.IndexOf(eff.enumNames, "None");
                eff.enumValueIndex = none >= 0 ? none : 0;
            }
            RequestInspectorRefresh(root);
        });

        DrawField(ref pos, root, "TimeEffect");
        DrawField(ref pos, root, "TimeScale");
        DrawField(ref pos, root, "TimeDuration");
    }

    private static void DrawSingle(
        ref Rect pos,
        SerializedProperty root,
        HashSet<ActionType> actions,
        ActionType type,
        string title,
        string field)
    {
        DrawSectionHeader(ref pos, title, () =>
        {
            actions.Remove(type);
            var f = root.FindPropertyRelative(field);
            if (f != null) f.objectReferenceValue = null;
            RequestInspectorRefresh(root);
        });

        DrawField(ref pos, root, field);
    }

    // ---------------- Menu ----------------

    private static void ShowAddMenu(SerializedProperty root, HashSet<ActionType> actions)
    {
        var menu = new GenericMenu();

        Add(menu, root, actions, ActionType.Particles, "Particles");
        Add(menu, root, actions, ActionType.Move, "Movement");
        Add(menu, root, actions, ActionType.Camera, "Camera");
        Add(menu, root, actions, ActionType.Sound, "Sound");
        Add(menu, root, actions, ActionType.Time, "Time Effect");
        Add(menu, root, actions, ActionType.Armament, "Armament");
        Add(menu, root, actions, ActionType.Castament, "Castament");

        menu.ShowAsContext();
    }

    private static void Add(GenericMenu menu, SerializedProperty root, HashSet<ActionType> actions, ActionType type, string label)
    {
        if (actions.Contains(type))
        {
            menu.AddDisabledItem(new GUIContent(label));
            return;
        }

        menu.AddItem(new GUIContent(label), false, () =>
        {
            actions.Add(type);
            RequestInspectorRefresh(root);
        });
    }
}
#endif
