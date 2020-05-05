using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Grid_Generator))]
public class GridGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Grid_Generator myScript = (Grid_Generator)target;
        if (GUILayout.Button("Generate Graph"))
        {
            myScript.GenerateGraph();
        }

    }
}
