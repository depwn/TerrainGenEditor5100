using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(CustomTerrain))]
[CanEditMultipleObjects]
public class CustomTerrainEditor : Editor
{
    //Property List
    SerializedProperty randomHeightRange;
    SerializedProperty perlinX;
    SerializedProperty perlinY;
    //menu folders
    bool randomHeight = false;
    bool perlinNoise = false;
    //to enable all changes as they are done
    private void OnEnable()
    {
        //setting the value to the CustomTerrain randomHeightRange
        randomHeightRange = serializedObject.FindProperty("randomHeightRange");
        perlinX = serializedObject.FindProperty("perlinX");
        perlinY = serializedObject.FindProperty("perlinY");
    }

    public override void OnInspectorGUI()
    {
        //updating values
        serializedObject.Update();
        //target is the linked script
        CustomTerrain cterrain = (CustomTerrain)target;
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
            EditorGUILayout.Slider(perlinX, 0, 1, new GUIContent("X"));//min 0 max 1
            EditorGUILayout.Slider(perlinY, 0, 1, new GUIContent("Y"));
            if (GUILayout.Button("PerlinNoise"))
            {
                cterrain.PerlinNoise(); //method in CustomTerrain
            }
        }
        if (GUILayout.Button("Reset Terrain"))
        {
            cterrain.ResetTerrain();
        }

        //applying them
        serializedObject.ApplyModifiedProperties();
    }
}
