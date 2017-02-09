using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class AutoSnap : EditorWindow
{
    // Add the menu item named "AutoSnap settings..." to the edit menu
    [MenuItem("Edit/AutoSnap settings...")]
    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(AutoSnap));
    }

    // set autosnapping to disabled on start
    private bool autoSnapEnabled = false;

    private Vector3 prevPosition;
    private float moveX=1, moveY=1, moveZ=1;

    // Creating the contents of the window
    void OnGUI()
    {
        GUILayout.Label("AutoSnap Settings", EditorStyles.boldLabel);
        autoSnapEnabled = EditorGUILayout.Toggle("AutoSnap enabled", autoSnapEnabled);
        moveX = EditorGUILayout.FloatField("Move X:", moveX);
        moveY = EditorGUILayout.FloatField("Move Y:", moveY);
        moveZ = EditorGUILayout.FloatField("Move Z:", moveZ);
    }

    // On every update
    void Update()
    {
        // if autosnapping is enabled
        if (autoSnapEnabled)
        {
            // And we selected something, and we are not in game mode
            if (Selection.transforms.Length > 0 & !EditorApplication.isPlaying)
            {
                // And the actual object actually moved position
                if (Selection.transforms[0].position != prevPosition) {
                    Snap();
                }
            }
        }
    }

    private void Snap()
    {
        try
        {
            for (int i = 0; i < Selection.transforms.Length; i++)
         {
                Vector3 t = Selection.transforms[i].transform.position;
                t.x = moveX * Mathf.Round((t.x / moveX));
                t.y = moveY * Mathf.Round((t.y / moveY));
                t.z = moveZ * Mathf.Round((t.z / moveZ));
                Selection.transforms[i].transform.position = t;
            }
            prevPosition = Selection.transforms[0].position;
        }
        catch (Exception e)
        {
            Debug.LogError("Nothing to move.  " + e);
        }
    }
}


