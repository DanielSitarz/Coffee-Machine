using Machine.Components;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(CoffeeMachine), true)]
[CanEditMultipleObjects]
public class CoffeeMachineEditor : Editor
{
    Container[] containers;

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal("controls");
        if (GUILayout.Button("On/Off"))
        {
            foreach (CoffeeMachine machine in targets) machine.ToggleOnOff();
        }
        if (GUILayout.Button("Brew Coffee"))
        {
            foreach (CoffeeMachine machine in targets) machine.Brew();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("settings");
        if (GUILayout.Button("Change Strength"))
        {
            foreach (CoffeeMachine machine in targets) machine.ChangeCoffeeStrength();
        }
        if (GUILayout.Button("Change Size"))
        {
            foreach (CoffeeMachine machine in targets) machine.ChangeCoffeeSize();
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal("containers");
        if (GUILayout.Button("Fill all containers"))
        {
            foreach (CoffeeMachine machine in targets) FillAllContainers(machine);
        }
        if (GUILayout.Button("Empty all containers"))
        {
            foreach (CoffeeMachine machine in targets) EmptyAllContainers(machine);
        }
        GUILayout.EndHorizontal();

        DrawDefaultInspector();
    }

    private void FillAllContainers(CoffeeMachine cm)
    {
        var containers = cm.GetComponentsInChildren<Container>();
        foreach (var container in containers) container.Fill(container.MaxCapacity);
    }

    private void EmptyAllContainers(CoffeeMachine cm)
    {
        var containers = cm.GetComponentsInChildren<Container>();
        foreach (var container in containers) container.Take(container.MaxCapacity);
    }
}