using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CollectiblesText : MonoBehaviour
{
    //Have different 'pages' - list
    //Display title, text and object collectible has been found
    //Display default message if collectible not found
    [SerializeField] int listPage;

    [SerializeField] private CollectibleDisplay defaultDisplay;
    [SerializeField] private List<CollectibleDisplay> collectibleDisplays;

    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descText;
    [SerializeField] private TextMeshProUGUI numberText;
    [SerializeField] private GameObject displayObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdatePage();
    }

    public void NextPage()
    {
        listPage++;
        if(listPage > (collectibleDisplays.Count - 1))
        {
            listPage = 0;
        }
        UpdatePage();
    }

    public void PrevPage()
    {
        listPage--;
        if (listPage < 0)
        {
            listPage = (collectibleDisplays.Count - 1);
        }
        UpdatePage();
    }

    private void SwapObject(GameObject obj)
    {
        GameObject newObj = Instantiate(obj, displayObj.transform);
        newObj.transform.SetParent(displayObj.transform.parent);
        newObj.transform.localScale = displayObj.transform.localScale;

        Destroy(displayObj);
        displayObj = newObj;
    }

    private void SetDefault()
    {
        nameText.text = defaultDisplay.display.name;
        descText.text = defaultDisplay.display.description;
        SwapObject(defaultDisplay.display.displayObject);
    }

    private void UpdatePage()
    {
        
        numberText.text = (listPage + 1).ToString();
        nameText.text = collectibleDisplays[listPage].display.name;
        descText.text = collectibleDisplays[listPage].display.description;
        SwapObject(collectibleDisplays[listPage].display.displayObject);

        if (!CollectibleManager.instance)
        {
            SetDefault();
            return;
        }

        switch (listPage)
        {
            case 0:
                if (!CollectibleManager.instance.c0)
                {
                    SetDefault();
                }
                break;
            case 1:
                if (!CollectibleManager.instance.c1)
                {
                    SetDefault();
                }
                break;
            case 2:
                if (!CollectibleManager.instance.c2)
                {
                    SetDefault();
                }
                break;
        }
    }
}
