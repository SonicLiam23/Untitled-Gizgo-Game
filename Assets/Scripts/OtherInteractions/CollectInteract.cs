using UnityEngine;

public class CollectInteract : MonoBehaviour, IInteractable
{
    [SerializeField] AudioClip sound; //Sound when picked up
    public int collectibleNum;

    void Start()
    {
        //check if this collectible number has already been collected
        //Destroy itself if it has

        switch (collectibleNum)
        {
            case 0:
                if (CollectibleManager.instance.c0)
                {
                    Destroy(gameObject);
                }
                break;
            case 1:
                if (CollectibleManager.instance.c1)
                {
                    Destroy(gameObject);
                }
                break;
            case 2:
                if (CollectibleManager.instance.c2)
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    public GameObject OnEnterRadius(Collider other)
    {
        Debug.Log("Entered bone zone");
        return other.gameObject;
    }

    public GameObject OnExitRadius(Collider other)
    {
        Debug.Log("exited bone zone");
        return other.gameObject;
    }

    public void OnInteract(GameObject interacter)
    {
        CollectibleManager.instance.SetCollectibles(collectibleNum);
        Debug.Log("Collected the bone!");
        AudioManager.instance.PlaySound(sound);
        Destroy(gameObject);
    }
}
