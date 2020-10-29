using Machine.Components;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CoffeeMachine), true)]
[CanEditMultipleObjects]
public class CoffeeMachineEditor : Editor
{
    CoffeeMachine machine;
    Container[] containers;

    void OnEnable()
    {
        CoffeeMachine machine = (CoffeeMachine)target;
        containers = machine.GetComponentsInChildren<Container>();
    }

    public override void OnInspectorGUI()
    {
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
        if (GUILayout.Button("Change Strength"))
        {
            machine.ChangeCoffeeStrength();
        }
        if (GUILayout.Button("Change Size"))
        {
            machine.ChangeCoffeeSize();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("containers");
        if (GUILayout.Button("Fill all containers"))
        {
            FillAllContainers();
        }
        if (GUILayout.Button("Empty all containers"))
        {
            EmptyAllContainers();
        }
        GUILayout.EndHorizontal();

        DrawDefaultInspector();
    }

    private void FillAllContainers()
    {
        foreach (var container in containers) container.Fill(container.MaxCapacity);
    }

    private void EmptyAllContainers()
    {
        foreach (var container in containers) container.Take(container.MaxCapacity);
    }
}