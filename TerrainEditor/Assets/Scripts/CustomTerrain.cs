using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 
using System.Linq;

[ExecuteInEditMode]
public class CustomTerrain : MonoBehaviour
{
    
    public Vector2 randomHeightRange = new Vector2(0f, 0.1f); //preset terrain height value=600f
    public Terrain terrain;
    public TerrainData terrainData;

    //Perlin
    public float perlinX = 0.01f;
    public float perlinY = 0.01f;

    private void OnEnable()
    {
        Debug.Log("Creating Terrain");
        terrain = this.GetComponent<Terrain>();
        //accessing the terrains data
        terrainData = terrain.terrainData;
    }
    private void Awake()
    {
        SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(
            "ProjectSettings/TagManager.asset")[0]);
        SerializedProperty tagsProperties = tagManager.FindProperty("tags");

        AddTag(tagsProperties, "Terrain");
        AddTag(tagsProperties, "Cloud");
        AddTag(tagsProperties, "Shore");
        //registering the new tags
        tagManager.ApplyModifiedProperties();

        this.gameObject.tag = "Terrain";
    }

    void AddTag(SerializedProperty tagsProperties, string newTag)
    {
        bool exists = false;
        //checking if tag exists
        for (int i=0; i<tagsProperties.arraySize; i++)
        {
            SerializedProperty t = tagsProperties.GetArrayElementAtIndex(i);
            if (t.stringValue.Equals(newTag))
            {
                exists = true;
                break;
            }
        }
        //inserting the newTag
        if (!exists)
        {
            tagsProperties.InsertArrayElementAtIndex(0);
            SerializedProperty newTagProperty = tagsProperties.GetArrayElementAtIndex(0);
            newTagProperty.stringValue = newTag;
        }
    }


    public void PerlinNoise()
    {
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                heightMap[x, y] = Mathf.PerlinNoise(x * perlinX, y * perlinY);
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    //heightmapResolution ---- heightmapWidth , heightmapHeight obsolete?
    public void RandomTerrain()
    {
        //float[,] heightMap;//creating height map
        //heightMap = new float[terrainData.heightmapResolution, terrainData.heightmapResolution]; // setting size
        float[,] heightMap = terrainData.GetHeights(0, 0, terrainData.heightmapResolution, terrainData.heightmapResolution);
        for (int i=0; i < terrainData.heightmapResolution; i++)
        {
            for (int j=0; j<terrainData.heightmapResolution; j++)
            {
                heightMap[i, j] += Random.Range(randomHeightRange.x, randomHeightRange.y); //making height changes on existing terrain
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void ResetTerrain()
    {
        //creating new empty height map
        float[,] heightMap;
        heightMap =new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        for (int i = 0; i < terrainData.heightmapResolution; i++)
        {
            for (int j = 0; j < terrainData.heightmapResolution; j++)
            {
                heightMap[i, j] = 0;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
}
