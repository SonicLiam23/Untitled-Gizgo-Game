using System;
using UnityEngine;


public enum BiomeType
{
    Path,
    Cold,
    Freezing
}


[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Scriptable Objects/BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    [Serializable]
    public class BiomeEntry
    {
        public BiomeType type;
        public GameObject prefab;
        public float distanceFromPath;
    }

    [SerializeField] public BiomeEntry[] biomes;

    public BiomeEntry GetBiome(float distanceFromRoad)
    {
        foreach (BiomeEntry biome in biomes)
        {
            if(distanceFromRoad < biome.distanceFromPath)
            {
                return biome;
            }
        }

        return biomes[biomes.Length - 1];
    }

}
