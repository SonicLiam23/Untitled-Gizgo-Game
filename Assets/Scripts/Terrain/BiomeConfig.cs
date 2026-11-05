using System;
using UnityEngine;

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

[CreateAssetMenu(fileName = "BiomeConfig", menuName = "Scriptable Objects/BiomeConfig")]
public class BiomeConfig : ScriptableObject
{
    [Serializable]
    public class BiomeEntry
    {
        public BiomeType Type;
        public float TempRate;
        public float HungerRate;
        public float PlayerSpeedIncrease;

        public AmountOfResources FoodResource;
        public AmountOfResources SticksResource;
        public AmountOfResources CampfiresResource;
        public AmountOfResources Trees;
    }

    [SerializeField] public BiomeEntry[] biomes;
}
