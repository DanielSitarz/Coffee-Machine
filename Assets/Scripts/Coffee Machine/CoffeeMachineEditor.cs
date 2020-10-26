using Machine.Components;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CoffeeMachine), true)]
[CanEditMultipleObjects]
public class CoffeeMachineEditor : Editor
{
    public override void OnInspectorGUI()
    {
        CoffeeMachine machine = (CoffeeMachine)target;

        GUILayout.BeginHorizontal("controls");
        if (GUILayout.Button("On/Off"))
        {
            machine.ToggleOnOff();
        }
        if (GUILayout.Button("Brew Coffee"))
        {
            machine.Brew();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("settings");
        if (GUILayout.Button("Power"))
        {
            machine.ChangeCoffeePower();
        }
        if (GUILayout.Button("Size"))
        {
            machine.ChangeCoffeeSize();
        }
        GUILayout.EndHorizontal();

        DrawDefaultInspector();
    }
}