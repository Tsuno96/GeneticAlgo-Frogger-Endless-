using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlayerManager))]
class ButtonWatch : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        if (GUILayout.Button("WatchBest"))
            PlayerManager.instance.WatchBest();

        if (GUILayout.Button("WatchAll"))
            PlayerManager.instance.WatchAll();
    }
}
