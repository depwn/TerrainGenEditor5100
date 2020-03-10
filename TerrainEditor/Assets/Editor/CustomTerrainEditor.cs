using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;
using System.IO;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
 
    //Property List-- explained in CustomTerrain as well
    SerializedProperty randomHeightRange;
    SerializedProperty resetTerrain; //default yes--untick in order to add to current HeightMap instead of resetting on each generation
    SerializedProperty perlinX;
    SerializedProperty perlinY;
    SerializedProperty perlinXoffset;
    SerializedProperty perlinYoffset;
    SerializedProperty perlinOctaves;
    SerializedProperty perlinPersistance;
    SerializedProperty perlinHeightScale;
    
    GUITableState perlinParametersTable;//using EditorGUITable (Unity Store Tool)
    SerializedProperty perlinParameters;
    //inspector menu folders 
    bool randomHeight = false;
    bool perlinNoise = false;
    bool multiplePerlinNoise = false;

    //to enable all changes as they are done
    private void OnEnable()
    {
        
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        resetTerrain = serializedObject.FindProperty("resetTerrain");
        perlinX = serializedObject.FindProperty("perlinX");
        perlinY = serializedObject.FindProperty("perlinY");
        perlinXoffset = serializedObject.FindProperty("perlinXoffset");
        perlinYoffset = serializedObject.FindProperty("perlinYoffset");
        perlinOctaves = serializedObject.FindProperty("perlinOctaves");
        perlinPersistance = serializedObject.FindProperty("perlinPersistance");
        perlinHeightScale = serializedObject.FindProperty("perlinHeightScale");
        perlinParametersTable = new GUITableState("perlinParametersTable");
        perlinParameters = serializedObject.FindProperty("perlinParameters");

    }

    public override void OnInspectorGUI()
    {
      
        //updating values
        serializedObject.Update();
        //target is the linked script
        CustomTerrain customTerrain = (CustomTerrain)target;
        

        Event e = Event.current;
        switch (e.type)
        {
            case EventType.KeyDown:
                {
                    if (e.keyCode == (KeyCode.Alpha6))
                    {
                        customTerrain.SaveTerrain();
                    }
                    if (e.keyCode == (KeyCode.Alpha7))
                    {
                        customTerrain.RandomTerrain();
                    }
                    if (e.keyCode == (KeyCode.Alpha8))
                    {
                        customTerrain.PerlinNoise();
                    }
                    if (e.keyCode == (KeyCode.Alpha3))
                    {
                        customTerrain.MultiplePerlinNoise();
                    }
                    if (e.keyCode == (KeyCode.Alpha4))
                    {
                        customTerrain.AddPerlinNoise();
                    }
                    if (e.keyCode == (KeyCode.Alpha5))
                    {
                        customTerrain.RemovePerlinNoise();
                    }
                    if (e.keyCode == (KeyCode.Alpha1))
                    {
                        customTerrain.ResetTerrain();
                    }
                    if (e.keyCode == (KeyCode.Alpha0))
                    {
                        customTerrain.LoadTerrain();
                    }
                    break;
                }
        }
        EditorGUILayout.PropertyField(resetTerrain);
        //fold out button and fields
        randomHeight = EditorGUILayout.Foldout(randomHeight, "RandomHeight");
        if (randomHeight)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set a random height within range", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if (GUILayout.Button(new GUIContent("Random Height", "Random Height Terrain Generation")))
            {
                customTerrain.RandomTerrain(); 
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        perlinNoise = EditorGUILayout.Foldout(perlinNoise, "PerlinNoise");
        if (perlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Noise", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinX, 0, 0.1f, new GUIContent("X"));//min 0 max 0.1 although still 
            EditorGUILayout.Slider(perlinY, 0, 0.1f, new GUIContent("Y"));//gets too "spiky"
            EditorGUILayout.IntSlider(perlinXoffset, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(perlinYoffset, 0, 10000, new GUIContent("Y Offset"));
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 10f, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0f, 1f, new GUIContent("Height Scale"));

            if (GUILayout.Button(new GUIContent("Perlin Noise", "Simle Perlin Noise generation")))
            {
                customTerrain.PerlinNoise(); 
            }
        }
        multiplePerlinNoise = EditorGUILayout.Foldout(multiplePerlinNoise, "Multiple Perlin Noise");
        if (multiplePerlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);
            perlinParametersTable=GUITableLayout.DrawTable(perlinParametersTable,perlinParameters);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button(new GUIContent("+", "Adding an extra Perlin Curve")))
            {
                customTerrain.AddPerlinNoise();
            }
            if (GUILayout.Button(new GUIContent("-", "Removing the selected Perlin Curve")))
            {
                customTerrain.RemovePerlinNoise();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button(new GUIContent("Apply Multiple Perlin", "Applying Multiple Perlin Noise Curves")))
            {
                customTerrain.MultiplePerlinNoise();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button(new GUIContent("Reset Terrain", "will completely reset terrain")))
        {
            customTerrain.ResetTerrain();
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        if (GUILayout.Button(new GUIContent("Save Terrain", "Saving Terrain in Assets/Saves")))
        {
            customTerrain.SaveTerrain();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button(new GUIContent("Load Terrain", "Loading Terrain from Assets/Saves")))
        {
            customTerrain.LoadTerrain();
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button(new GUIContent("Help Window", "Will open Help Window")))
        {
            HelpWindow.ShowHelpWindow();
        }

        //applying modifications
        serializedObject.ApplyModifiedProperties();
    }

}
