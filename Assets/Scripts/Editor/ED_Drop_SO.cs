using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Drop_SO))]
public class ED_Drop_SO : Editor
{
    private Drop_SO targetScript;
    private bool showElementsPercentage;

    private void OnEnable()
    {
        targetScript = (Drop_SO)target;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (targetScript.totalDrops < targetScript.dropTable.Length || GUILayout.Button("Calculate weight"))
        {
            CalculateTotalWeight();
        }
        EditorGUILayout.BeginVertical("GroupBox");

        GUI.enabled = false;
        EditorGUI.indentLevel++;
        showElementsPercentage = EditorGUILayout.Foldout(showElementsPercentage, "Elements");
        if (showElementsPercentage)
        {
            EditorGUI.indentLevel++;
            for (int i = 0; i < targetScript.dropTable.Length; i++)
            {
                EditorGUILayout.TextField($"{targetScript.dropTable[i].editorName} : ", $"{targetScript.dropTable[i].weight / targetScript.totalWeight * 100:F0} % ");
            }
            EditorGUI.indentLevel--;
        }
        EditorGUI.indentLevel--;

        EditorGUILayout.TextField("Total weight : ", $"{targetScript.totalWeight}");
        EditorGUILayout.TextField("Total drops : ", $"{targetScript.totalDrops}");

        GUI.enabled = true;

        EditorGUILayout.EndVertical();
    }

    private void CalculateTotalWeight()
    {
        targetScript.totalDrops = targetScript.dropTable.Length;
        targetScript.totalWeight = 0;
        foreach (Drop_SO.DropWithWeight drop in targetScript.dropTable)
        {
            targetScript.totalWeight += drop.weight;
        }
    }
}
