using UnityEngine;
using UnityEditor;

namespace Kit
{
    public class EditorUtils
    {
        [MenuItem("GameObject/Create Empty At Zero #%&n", priority = 0)]
        public static void CreateEmptyAtZero()
        {
            GameObject empty = new GameObject("GameObject");
            Undo.RegisterCreatedObjectUndo(empty, "Create new empty");
            empty.transform.SetParent(Selection.activeTransform);
            empty.transform.localPosition = Vector3.zero;
            Selection.objects = new Object[] { empty };
        }
    }
}