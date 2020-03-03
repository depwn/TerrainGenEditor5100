using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor; 
using System.Linq;
using System.IO;

[ExecuteInEditMode]
public class CustomTerrain : MonoBehaviour
{

    public Vector2 randomHeightRange = new Vector2(0f, 0.1f); //preset terrain height value=600f
    public Terrain terrain;
    public TerrainData terrainData;
    public bool resetTerrain = true; //found in editor as well default to reset terrain-- untick to not reset and add to each height map
    //private string path;
    //Perlin
    public float perlinX = 0.01f;
    public float perlinY = 0.01f;
    public int perlinXoffset = 0;
    public int perlinYoffset = 0;
    public int perlinOctaves = 3;//the less the smoother it will be
    public float perlinPersistance = 8f; //difference between perlin curves --- lowering the persistance will make the map flater
    public float perlinHeightScale = 0.09f;

    //multiplePerlin
    [System.Serializable]
    public class PerlinParameters
    {
        public float mPerlinX = 0.01f;
        public float mPerlinY = 0.01f;
        public int mPerlinXoffset = 0;
        public int mPerlinYoffset = 0;
        public int mPerlinOctaves = 3;
        public float mPerlinPersistance = 8f;
        public float mPerlinHeightScale = 0.09f;
        public bool remove = false;
    }
    public List<PerlinParameters> perlinParameters = new List<PerlinParameters>()
    {
    new PerlinParameters() //for the GUI table-- can't be empty
    };

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
    public float[,] GetHeightMap()
    {
        if (!resetTerrain)
        {
            return terrainData.GetHeights(0, 0, terrainData.heightmapResolution,terrainData.heightmapResolution);
        }
        else
        {
            return new float[terrainData.heightmapResolution, terrainData.heightmapResolution];
        }
    }


    public void PerlinNoise()
    {
        float[,] heightMap = GetHeightMap();
        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                
                heightMap[x, y] += Utilities.fBM((x+perlinXoffset) * perlinX, (y+perlinYoffset) * perlinY, perlinOctaves, perlinPersistance) * perlinHeightScale;
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void MultiplePerlinNoise()
    {
        float[,] heightMap = GetHeightMap();
        for (int y = 0; y < terrainData.heightmapResolution; y++)
        {
            for (int x = 0; x < terrainData.heightmapResolution; x++)
            {
                //adding all the PerlinNoise Curves
                foreach (PerlinParameters t in perlinParameters)
                {
                    heightMap[x, y] += Utilities.fBM((x + t.mPerlinXoffset) * t.mPerlinX, (y + t.mPerlinYoffset) * t.mPerlinY, t.mPerlinOctaves, t.mPerlinPersistance) * t.mPerlinHeightScale;
                }
            }
        }
        terrainData.SetHeights(0, 0, heightMap);
    }
    public void AddPerlinNoise()//adding an extra perlin in the perlins list
    {
        perlinParameters.Add(new PerlinParameters());
    }
    public void RemovePerlinNoise()//removing 
    {
        //copying to new and removing not wanted items
        List<PerlinParameters> remainingPerlinParameters = new List<PerlinParameters>(); 
        for (int i=0; i<perlinParameters.Count; i++)
        {
            if (!perlinParameters[i].remove)
            {
                remainingPerlinParameters.Add(perlinParameters[i]);//adding all that are not selected for removal
            }
        }
        if (remainingPerlinParameters.Count == 0)//ensuring list is not empty
        {
            remainingPerlinParameters.Add(perlinParameters[0]);//if empty adding an item -- GUI table cant be empty
        }
        perlinParameters = remainingPerlinParameters;//copy
    }


    public void RandomTerrain()
    {
        
        float[,] heightMap = GetHeightMap();
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
    public void SaveTerrain(/*path*/)
    {
        AssetDatabase.CreateAsset(terrainData, "Assets/Saves" + terrain.name + ".asset");
    }
    
    public static void SaveMeshInPlace(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, false, true);
    }

    public static void SaveMeshNewInstanceItem(MenuCommand menuCommand)
    {
        MeshFilter mf = menuCommand.context as MeshFilter;
        Mesh m = mf.sharedMesh;
        SaveMesh(m, m.name, true, true);
    }

    public static void SaveMesh(Mesh mesh, string name, bool makeNewInstance, bool optimizeMesh)
    {
        string path = EditorUtility.SaveFilePanel("Save Separate Mesh Asset", "Assets/", name, "asset");
        if (string.IsNullOrEmpty(path)) return;

        path = FileUtil.GetProjectRelativePath(path);

        Mesh meshToSave = (makeNewInstance) ? Object.Instantiate(mesh) as Mesh : mesh;

        if (optimizeMesh)
            MeshUtility.Optimize(meshToSave);

        AssetDatabase.CreateAsset(meshToSave, path);
        AssetDatabase.SaveAssets();
    }
}
