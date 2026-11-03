using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] AudioSource audio;

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

    public void PlaySound(AudioClip sound)
    {
        audio.clip = sound;
        audio.Play();
    }
}
