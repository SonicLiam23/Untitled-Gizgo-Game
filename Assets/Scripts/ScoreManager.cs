using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public static ScoreManager Instance { get; private set; } = null;

    public TMP_Text meatText;
    public TMP_Text plantText;
    public TMP_Text sticksText;

    private int meatCount = 0;
    private int plantCount = 0;
    private int sticksCount = 0;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void AddMeat()
    {
        meatCount++;
        meatText.text = "Meats: " + meatCount.ToString(); 
    }

    public void AddPlant()
    {
        plantCount++;
        plantText.text = "Plants: " + plantCount.ToString();
    }

    public void AddSticks()
    {
        sticksCount++;
        sticksText.text = "Sticks: " + sticksCount.ToString();
    }

    public void RemoveSticks(int numberToRemove)
    {
        sticksCount -= numberToRemove;
        sticksText.text = "Sticks: " + sticksCount.ToString();
    }

    public int GetSticks()
    {
        return sticksCount;
    }
}
