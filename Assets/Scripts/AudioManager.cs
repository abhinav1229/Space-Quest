using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource Ice;
    public AudioSource Fire;
    public AudioSource Hit;
    public AudioSource Pause;
    public AudioSource Unpause;

    private void Awake() {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioSource sound)
    {
        sound.Stop();
        sound.Play();
    }
}
