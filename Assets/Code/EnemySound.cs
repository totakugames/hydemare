using UnityEngine;

public class EnemySound : MonoBehaviour
{

    private const string TAG_PLAYER = "Player";

    [SerializeField] private AudioClip enemyWalkSound;

    private AudioSource audioSource;

    private bool isPlayerNearby = false;
    private bool canPlay = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        //Invoke("EnableSound", 0.1f);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(TAG_PLAYER))
        {
            isPlayerNearby = true;
            Debug.Log("Player entered hearing range!");
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(TAG_PLAYER))
        {
            isPlayerNearby = false;
            Debug.Log("Player exited hearing range!");
        }
    }

    // Function called by animation event
    public void PlayEnemyWalkSound()
    {
        Debug.Log("PlayFlapSound called! PlayerNearby: " + isPlayerNearby);

        if (isPlayerNearby)
        {
            audioSource.PlayOneShot(enemyWalkSound);
            Debug.Log("Playing sound!");
        }
    }
/*

    public void PlayEnemyWalkSound()
    {
        Debug.Log("=== FORCE PLAY TEST ===");
        if (enemyWalkSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(enemyWalkSound, 1.0f); // Volume auf 1
            Debug.Log("Sound should play NOW!");
        }
        else
        {
            Debug.Log("PROBLEM: flapSound null? " + (enemyWalkSound == null) + " audioSource null? " + (audioSource == null));
        }
    }
    */
}