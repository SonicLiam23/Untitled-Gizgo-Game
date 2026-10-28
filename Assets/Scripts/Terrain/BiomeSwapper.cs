using System;
using UnityEngine;

public class BiomeSwapper : MonoBehaviour
{
    private TerrainChecker checker;
    private string currentLayer;

    [SerializeField] private BiomeConfig biomesConfig;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        checker = new TerrainChecker();
    }

    public BiomeConfig.BiomeEntry CheckBiomeAtPos(Vector3 checkedPos)
    {
        RaycastHit hit;

        BiomeType parsedType;

        if (Physics.Raycast(checkedPos, Vector3.down, out hit, 3))
        {
            //if terrain exists
            if(hit.transform.GetComponent<Terrain>() != null)
            {
                Terrain t = hit.transform.GetComponent<Terrain>();

                //if terrain is not the same as the current terrain
                if (currentLayer != checker.GetLayerName(checkedPos, t))
                {
                    currentLayer = checker.GetLayerName(checkedPos, t);

                    //convets the string name of currentLayer to a BiomeType value and checks if they are the same
                    if(Enum.TryParse<BiomeType>(currentLayer, true, out parsedType))
                    {
                        foreach (BiomeConfig.BiomeEntry biome in biomesConfig.biomes)
                        {
                            if (biome.Type == parsedType)
                            {
                                return biome;
                            }
                        }
                    }
                }
            }
        }

        //Return the current biome in case it did not change
        if (Enum.TryParse<BiomeType>(currentLayer, true, out parsedType))
        {
            foreach (BiomeConfig.BiomeEntry biome in biomesConfig.biomes)
            {
                if (biome.Type == parsedType)
                {
                    return biome;
                }
            }
        }

        return null;
    }
}
