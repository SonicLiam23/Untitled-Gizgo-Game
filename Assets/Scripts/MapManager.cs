using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    // [SerializeField] public int mapWidth;
    // [SerializeField] public int mapDepth;
    //  public Transform floorContainer;
    //  public Transform wallContainer;
    //   public Transform interactablesContainer;
    //  [SerializeField] public GameObject mapEdgePrefab;
    //  public float wobbleAmount = 8f;
    //  public float magnitude;

    //  [SerializeField] int seed;
    //  private int xOffset, zOffset;


    [SerializeField] private Tilemap tilemap;
    private Dictionary<Vector3Int, TileBase> tiles = new Dictionary<Vector3Int, TileBase>();

    public BiomeConfig biomeConfig;
    public ItemsConfig itemConfig;

    private System.Random random = new System.Random();

    //BiomeConfig.BiomeEntry[,] terrainObjects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        /*if (seed != 0)
        {
            random = new System.Random(seed);
        }
        else
        {
            random = new System.Random();
        }

        terrainObjects = new BiomeConfig.BiomeEntry[mapWidth, mapDepth];

        xOffset = random.Next(-10000, 10000);
        zOffset = random.Next(-10000, 10000);*/


        PopulateTileArray();


        //GameManager.Instance.Elephant.SetActive(false);
        // GameManager.Instance.Human.SetActive(false);
        // GenerateMap();
        // PopulateMap();
        //GameManager.Instance.Elephant.SetActive(true);
        //GameManager.Instance.Human.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void PopulateTileArray()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach(Vector3Int position in bounds.allPositionsWithin)
        {
            TileBase tile = tilemap.GetTile(position);

            if (tile != null)
            {
                tiles.Add(position, tile);
            }
        }
    }

    public BiomeTile GetBiomeTileAtPosition(Vector3 worldPosition)
    {
        worldPosition.y = 0f;

        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        return tilemap.GetTile<BiomeTile>(cellPosition);
    }

    /* void GenerateMap()
     {
         for (int x = 0; x < mapWidth; x++)
         {
             for (int z = 0; z < mapDepth; z++)
             {
                 //Adjusts the path area so it gets placed along the middle of the map
                 float distance = Mathf.Abs(z - mapDepth / 2f);

                 //Perlin Noise
                 float noise = Mathf.PerlinNoise(x * magnitude + xOffset, z * magnitude + zOffset) - 0.5f;
                 float distanceFromPath = distance + noise * wobbleAmount;

                 BiomeConfig.BiomeEntry biome = biomeConfig.GetBiome(distanceFromPath);
                 GameObject floorObject = biome.Prefab;
                 GameObject floorInstance = Instantiate(floorObject, new Vector3(x, 0, z), Quaternion.identity);
                 floorInstance.name = string.Format("floorObj_x{0}_z{1}", x, z);
                 floorInstance.transform.SetParent(floorContainer, false);

                 terrainObjects[x, z] = biome;

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
         for (int x = 0; x < mapWidth; x++)
         {
             for (int z = 0; z < mapDepth; z++)
             {
                 switch (terrainObjects[x, z].Type)
                 {
                     case BiomeType.Path:

                         ChooseItem(50, x, z);
                         break;

                     case BiomeType.Cold:

                         ChooseItem(20, x, z);
                         break;
                     case BiomeType.Freezing:

                         ChooseItem(10, x, z);
                         break;
                     default:

                         break;
                 }

             }
         }
     }



     void ChooseItem(int chance, int x, int z)
     {
         if (random.Next(0, 10000) < chance)
         {
             int roll = random.Next(0, 5);

             switch (roll)
             {
                 case 0:

                     ItemsConfig.ItemEntry plantItem = itemConfig.GetItem(ItemType.Plant);
                     GameObject plantObject = plantItem.Prefab;
                     GameObject plantInstance = Instantiate(plantObject, new Vector3(x, 1, z), Quaternion.identity);

                     plantInstance.name = string.Format("plantObj_x{0}_z{1}", x, z);
                     plantInstance.transform.SetParent(interactablesContainer, false);

                     break;

                 case 1:

                     ItemsConfig.ItemEntry meatItem = itemConfig.GetItem(ItemType.Meat);
                     GameObject meatObject = meatItem.Prefab;
                     GameObject meatInstance = Instantiate(meatObject, new Vector3(x, 1, z), Quaternion.identity);

                     meatInstance.name = string.Format("meatObj_x{0}_z{1}", x, z);
                     meatInstance.transform.SetParent(interactablesContainer, false);

                     break;


                 case 2 or 3:

                     ItemsConfig.ItemEntry stickItem = itemConfig.GetItem(ItemType.Stick);
                     GameObject stickObject = stickItem.Prefab;
                     GameObject stickInstance = Instantiate(stickObject, new Vector3(x, 1, z), Quaternion.identity);

                     stickInstance.name = string.Format("stickObj_x{0}_z{1}", x, z);
                     stickInstance.transform.SetParent(interactablesContainer, false);


                     break;

                 case 4:

                     ItemsConfig.ItemEntry campfireItem = itemConfig.GetItem(ItemType.Campfire);
                     GameObject campfireObject = campfireItem.Prefab;
                     GameObject campfireInstance = Instantiate(campfireObject, new Vector3(x, 1, z), Quaternion.identity);

                     campfireInstance.name = string.Format("campfireObj_x{0}_z{1}", x, z);
                     campfireInstance.transform.SetParent(interactablesContainer, false);


                     break;
             }
         }
         else
         {
             return;
         }
     }*/
}
