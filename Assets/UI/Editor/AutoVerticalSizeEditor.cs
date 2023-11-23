using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AutomaticVerticalSize))]
public class AutoVerticalSizeEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if(GUILayout.Button("Recalc Size"))
        {
            AutomaticVerticalSize myScript=((AutomaticVerticalSize)target);
            myScript.AdjustSize();
        }
    }
}
