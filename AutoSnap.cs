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
    bool autoSnapEnabled = false;

    Vector3 prevPosition;
    float moveX=1, moveY=1, moveZ=1;

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
                    snap();
                }
            }
        }
    }


    private void snap()
    {
        try
        {
            for (int i = 0; i < Selection.transforms.Length; i++)
         {
                Vector3 t = Selection.transforms[i].transform.position;
                t.x = round(t.x, SnapAxis.X);
                t.y = round(t.y, SnapAxis.Y);
                t.z = round(t.z, SnapAxis.Z);
                Selection.transforms[i].transform.position = t;
            }
            prevPosition = Selection.transforms[0].position;
        }
        catch (Exception e)
        {
            Debug.LogError("Nothing to move.  " + e);
        }
    }

    private enum SnapAxis {X,Y,Z}

    private float round(float value, SnapAxis axis)
    {
        float _base = 0f;
        switch (axis)
        {
            case SnapAxis.X:
                _base = moveX;
                break;
            case SnapAxis.Y:
                _base = moveY;
                break;
            case SnapAxis.Z:
                _base = moveZ;
                break;
        }
        return _base * Mathf.Round((value / _base));
    }
}


