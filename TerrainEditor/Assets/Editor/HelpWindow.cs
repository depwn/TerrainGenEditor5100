using UnityEngine;
using UnityEditor;
using EditorGUITable;
using System.IO;
public class HelpWindow : EditorWindow
{

    public static void ShowHelpWindow()
    {

        HelpWindow window = EditorWindow.GetWindow<HelpWindow>();
    }
    private void OnGUI()
    {

        RandomHeightInfo();
        ResetTerrainInfo();
        PerlinNoiseInfo();
        MultiplePerlinNoiseInfo();
        SaveTerrainInfo();
        LoadTerrainInfo();
        ShortcutInfo();
    }


    private void RandomHeightInfo()
    {
        EditorGUILayout.BeginVertical();
        {

            EditorGUILayout.LabelField("Random HeightInfo", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Random Height Button will generate a terrain with random height values(no perlin)");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    private void ResetTerrainInfo()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Reset Terrain Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("The Reset Terrain button will completely reset the current terrain flattening all heights");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    private void PerlinNoiseInfo()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("PerlinNoiseInfo", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("The Perlin Noise Button will apply a single Perlin Noise Curve and generate Terrain");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    private void MultiplePerlinNoiseInfo()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("MultiplePerlinNoiseInfo", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("the + Button will add an exta Perlin Noise Curve for better terrain generation");
            EditorGUILayout.LabelField("the - Button will remove the selected Perlin Noise Curve");
            EditorGUILayout.LabelField("the Remove checkbox selects which curve(s) u want to remove");
            EditorGUILayout.LabelField("the Apply Multiple Terrain Button finalizes creation");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }

    private void SaveTerrainInfo()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Save Terrain Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Will Save current Terrain at path Assets/Saves");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    private void LoadTerrainInfo()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Load Terrain Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Will load the most recent saved Terrain");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    private void ShortcutInfo()
    {
        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.LabelField("Keyboard Shortcut Info", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("For they Shortcuts to work u need to click on an option in the Inspector (e.g. on a foldout ");
            EditorGUILayout.LabelField("Reset Terrain: Alpha1 ");
            EditorGUILayout.LabelField("Multiple Perlin Confirmation Button : Alpha3");
            EditorGUILayout.LabelField("Add Perlin Noise(extra noise curve): Alpha4 ");
            EditorGUILayout.LabelField("Remove Perlin(remove noise curve): Alpha5");
            EditorGUILayout.LabelField("Save Terrain: Alpha6 ");
            EditorGUILayout.LabelField("Random Terrain Generation: Alpha7 ");
            EditorGUILayout.LabelField("Single Perlin Noise Terrain: Alpha8 ");
            EditorGUILayout.LabelField("Load Terrain: Alpha0 ");
        }
        EditorGUILayout.EndVertical();
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
}
