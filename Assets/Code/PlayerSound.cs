using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioClip walkSound;
    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
    }

    public void PlayWalkSound()
    {
        if (walkSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(walkSound);
        }
    }

    public void StopWalkSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}