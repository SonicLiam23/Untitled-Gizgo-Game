using UnityEngine;
using UnityEngine.Tilemaps;

public enum BiomeType
{
    Path,
    Cold,
    Freezing
}

[CreateAssetMenu(fileName = "BiomeTile", menuName = "Tiles/Biome Tile")]
public class BiomeTile : Tile
{
    public BiomeType Type;
    public GameObject Prefab;
    public int TempRate;
    public int HungerRate;
}
