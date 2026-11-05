using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BiomeTile", menuName = "Tiles/Biome Tile")]
public class BiomeTile : Tile
{
    public BiomeType Type;
    public GameObject Prefab;
    public float TempRate;
    public float HungerRate;
    public float PlayerSpeedChange;
    public AmountOfResources FoodResource;
    public AmountOfResources SticksResource;
    public AmountOfResources CampfiresResource;
    public AmountOfResources Trees;

}
