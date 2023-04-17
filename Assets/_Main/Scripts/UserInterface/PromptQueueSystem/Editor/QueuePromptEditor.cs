using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

[CustomEditor(typeof(QueuePrompt), true)]
class QueuePromptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        QueuePrompt g = (QueuePrompt)target;

        g.queuePromptType = (QueuePromptType)EditorGUILayout.EnumPopup("Queue Prompt Type", g.queuePromptType);

        if (g.queuePromptType == QueuePromptType.Delayed)
        {
            g.delayDuration = EditorGUILayout.FloatField("Delay Duration", g.delayDuration);
        }

        EditorGUILayout.Space(10);
        DrawDefaultInspector();

        if (GUI.changed)
        {
            Undo.RecordObject(g, "Queue Prompt");
            serializedObject.ApplyModifiedProperties();
            PrefabUtility.RecordPrefabInstancePropertyModifications(g);
            EditorUtility.SetDirty(g);
        }

    }
}