using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


    [CustomEditor(typeof(MazeGenerator))]

public class EditorGenerator : Editor
{
    private MazeGenerator generator;
    // Start is called before the first frame update

    
    override public void OnInspectorGUI()
    {
        generator=GameObject.FindGameObjectWithTag("generator").GetComponent<MazeGenerator>();
        if (GUILayout.Button("Generate"))
        {
            generator.generateMaze();
        }
        DrawDefaultInspector();
    }
}
