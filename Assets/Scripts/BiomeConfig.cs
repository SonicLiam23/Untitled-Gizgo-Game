using System;
using UnityEngine;



[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Scriptable Objects/BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    [Serializable]
    public class BiomeEntry
    {
        //public BiomeType Type;
        public GameObject Prefab;
        //public float DistanceFromPath;
        public float TempRate;
        public float HungerRate;
    }

    [SerializeField] public BiomeEntry[] biomes;

    /*public BiomeEntry GetBiome(float distanceFromRoad)
    {
        foreach (BiomeEntry biome in biomes)
        {
            if(distanceFromRoad < biome.DistanceFromPath)
            {
                return biome;
            }
        }

        return biomes[biomes.Length - 1];
    }*/

}
