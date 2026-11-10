using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    bool hasPressedPlay = false;

    //Simple switch to another scene
    public void SwitchScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    //Quick and easy exit function
    public void ExitGame()
    {
        Application.Quit();
    }

    //Play sound and wait before switching scene
    public void SwitchSceneSound(string scene)
    {
        if(hasPressedPlay)
        {
            audioSource.Stop();
            return;
        }

        StartCoroutine(WaitForSound(scene));
        hasPressedPlay = true;
        
    }

    IEnumerator WaitForSound(string scene)
    {
        audioSource.Play();

        yield return new WaitUntil(() => !audioSource.isPlaying);

        Debug.Log("Sound finished");
        SceneManager.LoadScene(scene);
    }
}
