using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource Ice;
    public AudioSource Fire;
    public AudioSource Hit;
    public AudioSource Pause;
    public AudioSource Unpause;
    public AudioSource Boom2;
    public AudioSource HitRock;
    public AudioSource Shoot;

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

    public void PlayModifiedSound(AudioSource sound)
    {
        sound.pitch = Random.Range(0.7f, 1.3f);
        sound.Stop();
        sound.Play();
    }
}
