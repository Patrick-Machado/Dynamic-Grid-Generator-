using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(New_Node_IA))]
public class NewNodeIAEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        New_Node_IA myScript = (New_Node_IA)target;
        if (GUILayout.Button("Ativar/Desativar Node"))
        {
            myScript.SwitchActiveBtn();
        }

    }
}
