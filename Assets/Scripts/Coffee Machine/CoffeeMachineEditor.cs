using Machine.Components;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CoffeeMachine), true)]
[CanEditMultipleObjects]
public class CoffeeMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CoffeeMachine controlPanel = (CoffeeMachine)target;

        GUILayout.BeginHorizontal("controls");
        if (GUILayout.Button("On"))
        {
            controlPanel.TurnOn();
        }
        if (GUILayout.Button("On"))
        {
            controlPanel.TurnOff();
        }
        if (GUILayout.Button("Brew Coffee")) { }
        GUILayout.EndHorizontal();
        DrawDefaultInspector();
    }
}