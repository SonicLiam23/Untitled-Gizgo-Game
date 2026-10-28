using NUnit.Framework.Constraints;
using System.Collections.Generic;
using UnityEngine;

public class MinigameManager : MonoBehaviour
{

    [SerializeField] private int Width;
    [SerializeField] private int Depth;
    [SerializeField] private int Height;
    [SerializeField] private GameObject ObjectToSpawn;
    [SerializeField] private int AmountOfObjects;

    private List<GameObject> spawnedObjects;

    private float timer = 20f;
    private float timeRemaining;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timeRemaining = timer;
        spawnedObjects = new List<GameObject>();

        GameObject tmp;
        for (int i = 0; i < AmountOfObjects; i++)
        {
            tmp = Instantiate(ObjectToSpawn);
            tmp.SetActive(false);
            spawnedObjects.Add(tmp);
        }

        SpawnObjects();
    }

    // Update is called once per frame
    void Update()
    {
        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 0)
        {
            timeRemaining = timer;
            DeactivateAllObjects();
            SpawnObjects();
        }
    }

    public void SpawnObjects()
    {
        for(int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Depth; j++)
            {
                GameObject Object = GetSpawnedObject();

                if (Object != null)
                {
                    float PosX = Random.Range(-Width, Width);
                    float PosZ = Random.Range(-Depth, Depth);
                    Object.transform.position = new Vector3(PosX, Height, PosZ);
                    Object.SetActive(true);
                }
            }
        }
    }

    public GameObject GetSpawnedObject()
    {
        for (int i = 0; i < AmountOfObjects; i++)
        {
            if (!spawnedObjects[i].activeInHierarchy)
            {
                return spawnedObjects[i];
            }
        }

        return null;
    }

    public void DeactivateObject(GameObject Object)
    {
        Object.SetActive(false);
    }

    public void DeactivateAllObjects()
    {
        for (int i = 0; i < AmountOfObjects; i++)
        {
            spawnedObjects[i].SetActive(false); 
        }
    }
}
