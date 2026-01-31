using UnityEngine;
using System.Collections;

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
        Debug.Log("PlayEnemyWalkSound called! PlayerNearby: " + isPlayerNearby);

        if (isPlayerNearby)
        {
            audioSource.PlayOneShot(enemyWalkSound);

            //float soundLength = enemyWalkSound.length;
            //float fadeStartTime = soundLength - 0.1f;

            /*if (fadeStartTime > 0)
            {
                StartCoroutine(FadeOutAfterDelay(fadeStartTime, 0.1f));
            }*/
        }
    }

    IEnumerator FadeOutAfterDelay(float delay, float fadeDuration)
    {
        yield return new WaitForSeconds(delay);

        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
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