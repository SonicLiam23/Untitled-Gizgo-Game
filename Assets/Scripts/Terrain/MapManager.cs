using System;
using Unity.Mathematics;
using UnityEngine;
using Random = System.Random;


public class MapManager : MonoBehaviour
{
    [SerializeField] private bool EnableGeneration = true;

    [Header("CONFIGURE THRESHOLDS FOR THE AMOUNT OF RESOURCES")]
    [SerializeField] private float VeryLowValue;
    [SerializeField] private float LowValue;
    [SerializeField] private float MediumValue;
    [SerializeField] private float HighValue;

    [Header("OTHER SERIALIZABLES")]

    [SerializeField] int seed;
    [SerializeField] private BiomeConfig biomesConfig;
    [SerializeField] private ItemsConfig itemConfig;

    [SerializeField] private Transform interactablesContainer;

    [SerializeField] private Terrain terrain;

    private System.Random random = new System.Random();

    // [SerializeField] public int mapWidth;
    // [SerializeField] public int mapDepth;
    //  public Transform floorContainer;
    //  public Transform wallContainer;
    //   public Transform interactablesContainer;
    //  [SerializeField] public GameObject mapEdgePrefab;
    //  public float wobbleAmount = 8f;
    //  public float magnitude;
    //  private int xOffset, zOffset;
    //[SerializeField] private Tilemap tilemap;
    //private Dictionary<Vector3Int, TileBase> tiles = new Dictionary<Vector3Int, TileBase>();
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

    private void PopulateMap()
    {
        TerrainChecker checker = new TerrainChecker();
        Bounds bounds = terrain.terrainData.bounds;

        //Iterates through the terrain
        for(float x = bounds.min.x;  x <= bounds.max.x; x++)
        {
            for (float z = bounds.min.z; z <= bounds.max.z; z++)
            {
                float y = terrain.terrainData.GetInterpolatedHeight((x - bounds.min.x) / bounds.size.x, (z - bounds.min.z) / bounds.size.z);
                Vector3 WorldPos = new Vector3(x, y, z);

                string currentLayer;
                BiomeType parsedType;

                currentLayer = checker.GetLayerName(WorldPos, terrain);

                //convets the string name of currentLayer to a BiomeType value and checks if they are the same
                if (Enum.TryParse<BiomeType>(currentLayer, true, out parsedType))
                {   
                    foreach (BiomeConfig.BiomeEntry biome in biomesConfig.biomes)
                    {
                        if (biome.Type == parsedType)
                        {
                            Vector3 spawnPos = new Vector3(x, y, z);
                            SpawnItem(biome, spawnPos);
                        }
                    }
                }
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

    void SpawnItem(BiomeConfig.BiomeEntry biome, Vector3 Pos)
    {
        float chance = 0.1f;
        if (random.NextDouble() > chance)
        {
            return;
        }
        else
        {
            bool hasItemBeenSpawned = false;

            if (!hasItemBeenSpawned)
            {
                if (random.NextDouble() < GetSpawnChance(biome.FoodResource))
                {
                    ItemType itemType = random.NextDouble() < 0.5f ? ItemType.Plant : ItemType.Meat;

                    ItemsConfig.ItemEntry foodItem = itemConfig.GetItem(itemType);
                    GameObject foodObject = foodItem.Prefab;
                    GameObject foodInstance = Instantiate(foodObject, Pos + Vector3.up, Quaternion.identity);

                    foodInstance.name = string.Format("foodObj_x{0}_z{1}", Pos.x, Pos.z);
                    foodInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
                else if (random.NextDouble() < GetSpawnChance(biome.SticksResource))
                {
                    ItemsConfig.ItemEntry stickItem = itemConfig.GetItem(ItemType.Stick);
                    GameObject stickObject = stickItem.Prefab;
                    GameObject stickInstance = Instantiate(stickObject, Pos + Vector3.up, Quaternion.identity);

                    stickInstance.name = string.Format("stickObj_x{0}_z{1}", Pos.x, Pos.z);
                    stickInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
                else if (random.NextDouble() < GetSpawnChance(biome.CampfiresResource))
                {
                    ItemsConfig.ItemEntry campfireItem = itemConfig.GetItem(ItemType.Campfire);
                    GameObject campfireObject = campfireItem.Prefab;
                    GameObject campfireInstance = Instantiate(campfireObject, Pos + Vector3.up, Quaternion.identity);

                    campfireInstance.name = string.Format("campfireObj_x{0}_z{1}", Pos.x, Pos.z);
                    campfireInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
                else if (random.NextDouble() < GetSpawnChance(biome.Trees))
                {
                    ItemsConfig.ItemEntry treeItem = itemConfig.GetItem(ItemType.Tree);
                    GameObject treeObject = treeItem.Prefab;
                    GameObject treeInstance = Instantiate(treeObject, Pos + Vector3.up, Quaternion.identity);

                    treeInstance.name = string.Format("treeObj_x{0}_z{1}", Pos.x, Pos.z);
                    treeInstance.transform.SetParent(interactablesContainer, false);

                    hasItemBeenSpawned = true;
                    return;
                }
            }
        }
    }

    /*void PopulateTileArray()
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
    }*/

    /* public BiomeTile GetBiomeTileAtPosition(Vector3 worldPosition)
     {
         Vector3Int cellPosition = tilemap.WorldToCell(worldPosition);

         return tilemap.GetTile<BiomeTile>(cellPosition);
     }*/

    /*void PopulateMap()
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
    }*/

}
