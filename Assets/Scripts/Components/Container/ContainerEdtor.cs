using Machine.Components;
using UnityEditor;
using UnityEngine;

namespace Machine
{
    [CustomEditor(typeof(Container), true)]
    [CanEditMultipleObjects]
    public class ContainerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            Container container = (Container)target;

            GUILayout.BeginHorizontal("buttons");
            if (GUILayout.Button("Fill Max"))
            {
                container.Fill(container.MaxCapacity);
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Empty"))
            {
                container.SetAmount(0);
                EditorUtility.SetDirty(target);
            }
            GUILayout.EndHorizontal();

            DrawDefaultInspector();
        }
    }
}