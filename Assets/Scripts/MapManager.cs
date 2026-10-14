using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.WSA;


public class MapManager : MonoBehaviour
{
    [SerializeField] private bool EnableGeneration = true;

    [Header("CONFIGURE THRESHOLDS FOR THE AMOUNT OF RESOURCES")]
    [SerializeField] private float VeryLowValue;
    [SerializeField] private float LowValue;
    [SerializeField] private float MediumValue;
    [SerializeField] private float HighValue;

    [Header("OTHER SERIALIZABLES")]

    // [SerializeField] public int mapWidth;
    // [SerializeField] public int mapDepth;
    //  public Transform floorContainer;
    //  public Transform wallContainer;
    //   public Transform interactablesContainer;
    //  [SerializeField] public GameObject mapEdgePrefab;
    //  public float wobbleAmount = 8f;
    //  public float magnitude;

    [SerializeField] int seed;
    //  private int xOffset, zOffset;


    [SerializeField] private Tilemap tilemap;
    private Dictionary<Vector3Int, TileBase> tiles = new Dictionary<Vector3Int, TileBase>();

    public BiomeConfig biomeConfig;
    public ItemsConfig itemConfig;

    [SerializeField] private Transform interactablesContainer;

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
        }*/

        if(EnableGeneration)
        {
            PopulateTileArray();
            PopulateMap();
        }
        

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

    void PopulateTileArray()
    {
        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
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
        Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

        return tilemap.GetTile<BiomeTile>(cellPosition);
    }

    void PopulateMap()
    {
        tilemap.CompressBounds();

        BoundsInt bounds = tilemap.cellBounds;

        foreach (Vector3Int position in bounds.allPositionsWithin)
        {
            if (!tilemap.HasTile(position))
            {
                continue;
            }

            int x = position.x;
            int z = position.z;
            int y = position.y;

            BiomeTile tile = tilemap.GetTile<BiomeTile>(position);

            if (tile != null)
            {
                SpawnItem(tile, tile.transform.GetPosition(), x, y, z);
            }
        }
    }

    float GetSpawnChance(AmountOfResources resource)
    {
        switch (resource)
        {
            case AmountOfResources.None:
                return 0;

            case AmountOfResources.VeryLow:
                return VeryLowValue;

            case AmountOfResources.Low:
                return LowValue;

            case AmountOfResources.Medium:
                return MediumValue;

            case AmountOfResources.High:
                return HighValue;

            default:
                return 0f;
        }
    }

    void SpawnItem(BiomeTile tile, Vector3 tilePos, int x, int y, int z)
    {
        float chance = 0.1f;
        if (Random.value > chance)
        {
            return;
        }
        else
        {
            bool hasItemBeenSpawned = false;

            if (!hasItemBeenSpawned)
            {
                if (Random.value < GetSpawnChance(tile.FoodResource))
                {
                    float roll = Random.value;
                    Debug.Log($"Food roll: {roll}, chance: {GetSpawnChance(tile.FoodResource)}");


                    ItemType itemType = Random.value < 0.5f ? ItemType.Plant : ItemType.Meat;

                    ItemsConfig.ItemEntry foodItem = itemConfig.GetItem(itemType);
                    GameObject foodObject = foodItem.Prefab;
                    GameObject foodInstance = Instantiate(foodObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    foodInstance.name = string.Format("foodObj_x{0}_z{1}", x, z);
                    foodInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
                else if (Random.value < GetSpawnChance(tile.SticksResource))
                {
                    ItemsConfig.ItemEntry stickItem = itemConfig.GetItem(ItemType.Stick);
                    GameObject stickObject = stickItem.Prefab;
                    GameObject stickInstance = Instantiate(stickObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    stickInstance.name = string.Format("stickObj_x{0}_z{1}", x, z);
                    stickInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
                else if (Random.value < GetSpawnChance(tile.CampfiresResource))
                {
                    ItemsConfig.ItemEntry campfireItem = itemConfig.GetItem(ItemType.Campfire);
                    GameObject campfireObject = campfireItem.Prefab;
                    GameObject campfireInstance = Instantiate(campfireObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    campfireInstance.name = string.Format("campfireObj_x{0}_z{1}", x, z);
                    campfireInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
                else if (Random.value < GetSpawnChance(tile.Trees))
                {
                    ItemsConfig.ItemEntry treeItem = itemConfig.GetItem(ItemType.Tree);
                    GameObject treeObject = treeItem.Prefab;
                    GameObject treeInstance = Instantiate(treeObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    treeInstance.name = string.Format("treeObj_x{0}_z{1}", x, z);
                    treeInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
            }
        }
    }
}

    
   /*void ChooseItem(BiomeType type, Vector3 tilePos)
    {
        if (random.Next(0, 100) < chance)
        {
            int roll = random.Next(0, 10);

            switch (roll)
            {
                case 0 or 1:

                    ItemsConfig.ItemEntry plantItem = itemConfig.GetItem(ItemType.Plant);
                    GameObject plantObject = plantItem.Prefab;
                    GameObject plantInstance = Instantiate(plantObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    plantInstance.name = string.Format("plantObj_x{0}_z{1}", x, z);
                    plantInstance.transform.SetParent(interactablesContainer, false);

                    break;

                case 2 or 3:

                    ItemsConfig.ItemEntry meatItem = itemConfig.GetItem(ItemType.Meat);
                    GameObject meatObject = meatItem.Prefab;
                    GameObject meatInstance = Instantiate(meatObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    meatInstance.name = string.Format("meatObj_x{0}_z{1}", x, z);
                    meatInstance.transform.SetParent(interactablesContainer, false);

                    break;


                case 4 or 5:

                    ItemsConfig.ItemEntry stickItem = itemConfig.GetItem(ItemType.Stick);
                    GameObject stickObject = stickItem.Prefab;
                    GameObject stickInstance = Instantiate(stickObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    stickInstance.name = string.Format("stickObj_x{0}_z{1}", x, z);
                    stickInstance.transform.SetParent(interactablesContainer, false);


                    break;

                case 6:

                    ItemsConfig.ItemEntry campfireItem = itemConfig.GetItem(ItemType.Campfire);
                    GameObject campfireObject = campfireItem.Prefab;
                    GameObject campfireInstance = Instantiate(campfireObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    campfireInstance.name = string.Format("campfireObj_x{0}_z{1}", x, z);
                    campfireInstance.transform.SetParent(interactablesContainer, false);


                    break;

                case 7 or 8 or 9:

                    ItemsConfig.ItemEntry treeItem = itemConfig.GetItem(ItemType.Tree);
                    GameObject treeObject = treeItem.Prefab;
                    GameObject treeInstance = Instantiate(treeObject, new Vector3(x, z + 1, y), Quaternion.identity);

                    treeInstance.name = string.Format("campfireObj_x{0}_z{1}", x, z);
                    treeInstance.transform.SetParent(interactablesContainer, false);


                    break;
            }
        }
        else
        {
            return;
        }
    }*/
       

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
}*/
