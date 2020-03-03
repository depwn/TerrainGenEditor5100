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
    SerializedProperty resetTerrain; //default yes--untick add to current HeightMap instead of resetting
    SerializedProperty perlinX;
    SerializedProperty perlinY;
    SerializedProperty perlinXoffset;
    SerializedProperty perlinYoffset;
    SerializedProperty perlinOctaves;
    SerializedProperty perlinPersistance;
    SerializedProperty perlinHeightScale;
    [SerializeField]
    private string path;

    GUITableState perlinParametersTable;//using EditorGUITable
    SerializedProperty perlinParameters;
    //menu folders
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
        CustomTerrain cterrain = (CustomTerrain)target;
        EditorGUILayout.PropertyField(resetTerrain);
        //fold out button and fields
        randomHeight = EditorGUILayout.Foldout(randomHeight, "RandomHeight");
        if (randomHeight)
        {
            //empty line for some spacing
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Set a random height within range", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(randomHeightRange);
            if (GUILayout.Button("Random Height"))
            {
                cterrain.RandomTerrain(); //method in CustomTerrain
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);

        perlinNoise = EditorGUILayout.Foldout(perlinNoise, "PerlinNoise");
        if (perlinNoise)
        {
            //empty line for some spacing
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Perlin Noise", EditorStyles.boldLabel);
            EditorGUILayout.Slider(perlinX, 0, 0.1f, new GUIContent("X"));//min 0 max 0.1 although still
            EditorGUILayout.Slider(perlinY, 0, 0.1f, new GUIContent("Y"));//gets too spiky
            EditorGUILayout.IntSlider(perlinXoffset, 0, 10000, new GUIContent("X Offset"));
            EditorGUILayout.IntSlider(perlinYoffset, 0, 10000, new GUIContent("Y Offset"));
            EditorGUILayout.IntSlider(perlinOctaves, 1, 10, new GUIContent("Octaves"));
            EditorGUILayout.Slider(perlinPersistance, 0.1f, 10f, new GUIContent("Persistance"));
            EditorGUILayout.Slider(perlinHeightScale, 0f, 1f, new GUIContent("Height Scale"));

            if (GUILayout.Button("PerlinNoise"))
            {
                cterrain.PerlinNoise(); //method in CustomTerrain
            }
        }
        multiplePerlinNoise = EditorGUILayout.Foldout(multiplePerlinNoise, "Multiple Perlin Noise");
        if (multiplePerlinNoise)
        {
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            GUILayout.Label("Multiple Perlin Noise", EditorStyles.boldLabel);
            perlinParametersTable=GUITableLayout.DrawTable(perlinParametersTable,perlinParameters);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                cterrain.AddPerlinNoise();
            }
            if (GUILayout.Button("-"))
            {
                cterrain.RemovePerlinNoise();
            }
            EditorGUILayout.EndHorizontal();
            if (GUILayout.Button("Apply Multiple Perlin"))
            {
                cterrain.MultiplePerlinNoise();
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Reset Terrain"))
        {
            cterrain.ResetTerrain();
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        if (GUILayout.Button("Save Terrain"))
        {
            cterrain.SaveTerrain();
        }

        //applying modifications
        serializedObject.ApplyModifiedProperties();
    }

}
