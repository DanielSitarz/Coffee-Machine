using UnityEditor;
using UnityEngine;

namespace Machine
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UniqueID))]
    public class UniqueIDEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            if (GUILayout.Button("Regenerate ID"))
            {
                if (EditorUtility.DisplayDialog(
                    "Regenerate ID?",
                    $"Are you sure to regenerate {targets.Length} IDs? Any saved data for this ids will be lost.",
                    "Yes", "No"))
                {
                    foreach (var ob in targets)
                    {
                        UniqueID uniqueID = (UniqueID)ob;
                        RegenerateID(uniqueID);
                    }
                }
            }

            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("uid"));
            EditorGUI.EndDisabledGroup();
        }

        private void RegenerateID(UniqueID uniqueID)
        {
            uniqueID.uid = UniqueID.GetUID();
            EditorUtility.SetDirty(uniqueID);
        }
    }
}
