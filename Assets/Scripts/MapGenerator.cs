using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] public int mapWidth;
    [SerializeField] public int mapDepth;

    public Transform floorContainer;
    public Transform wallContainer;
    public Transform interactablesContainer;

    public BiomeConfig biomeConfig;

    [SerializeField] public GameObject mapEdgePrefab;
    [SerializeField] public GameObject sticksPrefab;
    public float wobbleAmount = 8f;
    public float magnitude;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void GenerateMap()
    {
        for (int x = 0; x < mapWidth; x++)
        {
            for (int z = 0; z < mapDepth; z++)
            {
                //Adjusts the path area so it gets placed along the middle of the map
                float distance = Mathf.Abs(z - mapDepth / 2f);

                //Perlin Noise
                float noise = Mathf.PerlinNoise(x * magnitude, z * magnitude) - 0.5f;
                float distanceFromPath = distance + noise * wobbleAmount;

                GameObject floorObject = biomeConfig.GetPrefab(distanceFromPath);
                GameObject floorInstance = Instantiate(floorObject, new Vector3(x, 0, z), Quaternion.identity);
                floorInstance.transform.SetParent(floorContainer, false);

                //Map Edges Instantiation
                if (x < 1 || z < 1 || x >= mapWidth - 1 || z >= mapDepth - 1)
                {
                    GameObject wallObject = Instantiate(mapEdgePrefab, new Vector3(x, 1, z), Quaternion.identity);
                    wallObject.transform.SetParent(wallContainer, false);
                }
            }
        }
    }

    void PopulateMap()
    {

    }
}
