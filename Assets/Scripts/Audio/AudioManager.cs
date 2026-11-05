using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    AudioSource audio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        audio = GetComponent<AudioSource>();    

        DontDestroyOnLoad(gameObject);
    }

    public void PlaySound(AudioClip sound)
    {
        audio.clip = sound;
        audio.Play();
    }
}
