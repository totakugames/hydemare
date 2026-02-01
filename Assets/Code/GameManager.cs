using UnityEngine;


/*
 * 
 * How to use it my friends:
 * Weltenwechsel mit Swoosh
 * GameManager.Instance.SwitchWorld(false); // Rabenwelt
 * GameManager.Instance.SwitchWorld(true);  // Schwanenwelt
 * how to use it einzeln:
 * GameManager.Instance.PlaySwoosh();
 * GameManager.Instance.PlayRavenMusic();
 * 
 */

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] AudioClip swanWorldMusic;
    [SerializeField] AudioClip ravenWorldMusic;
    [SerializeField] AudioClip mainMenuMusic;
    [SerializeField] AudioClip swooshSound;

    private AudioSource musicSource;
    private AudioSource sfxSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = 0.5f;

        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        PlayMusic(swanWorldMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }

    public void SwitchWorld(bool toDark)
    {
        PlaySFX(swooshSound);
        
        if (toDark) 
        {
            PlayMusic(ravenWorldMusic);
        }
        else 
        {        
            PlayMusic(swanWorldMusic);
        }

        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Raven");
        foreach (GameObject obj in gameObjects)
        {
            Renderer objRender = obj.GetComponent<Renderer>();
            objRender.enabled = toDark;
        }
        
    }
}