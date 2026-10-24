using UnityEngine;

public class TerrainChecker
{
    private float[] GetTextureMix(Vector3 checkedPos, Terrain t)
    {
        Vector3 terrainPos = t.transform.position;
        TerrainData tData = t.terrainData;
        int mapX = Mathf.RoundToInt((checkedPos.x - terrainPos.x) / tData.size.x * tData.alphamapWidth);
        int mapZ = Mathf.RoundToInt((checkedPos.z - terrainPos.z) / tData.size.z * tData.alphamapHeight);

        float[,,] splatMapData = tData.GetAlphamaps(mapX, mapZ, 1, 1);

        float[] cellMix = new float[splatMapData.GetUpperBound(2) + 1];

        for (int i = 0; i < cellMix.Length; i++)
        {
            cellMix[i] = splatMapData[0, 0, i];
        }

        return cellMix;
    }


    public string GetLayerName(Vector3 checkedPos, Terrain t)
    {
        float[] cellMix = GetTextureMix(checkedPos, t);
        float strongest = 0;
        int maxIndex = 0;

        for (int i = 0; i < cellMix.Length; i++)
        {
            if (cellMix[i] > strongest)
            {
                maxIndex = i;
                strongest = cellMix[i];
            }
        }

        return t.terrainData.terrainLayers[maxIndex].name;
    }
}
