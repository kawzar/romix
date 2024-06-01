using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioSource source;
    
    public static AudioManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        source.PlayOneShot(clip);
    }
}
