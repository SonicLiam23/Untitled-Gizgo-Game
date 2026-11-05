using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;

    public bool c0;
    public bool c1;
    public bool c2;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void SetCollectibles(int num)
    {
        switch (num) 
        { 
            case 0:
                c0 = true;
                break;
            case 1:
                c1 = true;
                break;
            case 2:
                c2 = true;
                break;
        }
    }

}
