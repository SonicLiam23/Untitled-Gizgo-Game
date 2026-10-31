using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Switch to game scene
    public void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //Quick and easy exit function
    public void ExitGame()
    {
        Application.Quit();
    }
}
