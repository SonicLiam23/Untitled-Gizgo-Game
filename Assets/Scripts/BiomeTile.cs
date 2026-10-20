using UnityEngine;
using UnityEngine.Tilemaps;

public enum BiomeType
{
    Path,
    Cold,
    Freezing
}

public enum AmountOfResources
{
    None,
    VeryLow,
    Low,
    Medium,
    High
}

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
