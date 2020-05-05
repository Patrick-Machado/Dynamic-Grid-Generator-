using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(GridSaver))]
public class GridSaverEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GridSaver myScript = (GridSaver)target;
        if (GUILayout.Button("Save Level"))
        {
            myScript.SaveLevel();
        }
        if (GUILayout.Button("Load Level"))
        {
            myScript.LoadLevel();
        }
    }

}
