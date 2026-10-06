using System;
using UnityEngine;

[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Scriptable Objects/BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    [Serializable]
    public class BiomeEntry
    {
        public string name;
        public GameObject prefab;
        public float distanceFromPath;
    }

    [SerializeField] public BiomeEntry[] biomes;

    public GameObject GetPrefab(float distanceFromRoad)
    {
        foreach (BiomeEntry biome in biomes)
        {
            if(distanceFromRoad < biome.distanceFromPath)
            {
                return biome.prefab;
            }
        }

        return biomes[biomes.Length - 1].prefab;
    }

}
