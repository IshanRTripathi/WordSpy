using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(AlphabetData), false)]
[CanEditMultipleObjects]
[System.Serializable]
public class AlphabetDrawer : Editor
{
    private ReorderableList plain;
    private ReorderableList normal;
    private ReorderableList highlighted;
    private ReorderableList wrong;

    private void OnEnable()
    {
        init(ref plain, "AlphabetPlain", "AlphabetPlain");
        init(ref normal, "AlphabetNormal", "AlphabetNormal");
        init(ref highlighted, "AlphabetHighlighted", "AlphabetHighlighted");
        init(ref wrong, "AlphabetWrong", "AlphabetWrong");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        plain.DoLayoutList();
        normal.DoLayoutList();
        highlighted.DoLayoutList();
        wrong.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void init( ref ReorderableList list, string propertyName, string listLabel)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName), true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, listLabel);
        };

        var l = list;
        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;
            EditorGUI.PropertyField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("letter"), GUIContent.none);
            EditorGUI.PropertyField(new Rect(rect.x + 70, rect.y, rect.width - 60 - 30, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("image"), GUIContent.none);
        };
    }
}
