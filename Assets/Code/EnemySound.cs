using UnityEngine;
using System.Collections;

public class EnemySound : MonoBehaviour
{

    private const string TAG_PLAYER = "Player";

    [SerializeField] private AudioClip enemyWalkSound;

    private AudioSource audioSource;
    private CircleCollider2D hearingTrigger;

    private bool isPlayerNearby = false;
    private bool canPlay = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;
        //Invoke("EnableSound", 0.1f);

        hearingTrigger = GetComponent<CircleCollider2D>();

    }

    bool IsPlayerNearby()
    {
        if (hearingTrigger == null) return false;

        GameObject player = GameObject.FindGameObjectWithTag(TAG_PLAYER);

        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.transform.position);
        return distance <= hearingTrigger.radius;
    }

    /*
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
*/

    // Function called by animation event

    public void PlayEnemyWalkSound()
    {
        //Debug.Log("PlayEnemyWalkSound called! PlayerNearby: " + isPlayerNearby);

        /*
        if (isPlayerNearby)
        {
            audioSource.PlayOneShot(enemyWalkSound);

        }
        */

        if (IsPlayerNearby())
        {
            audioSource.PlayOneShot(enemyWalkSound);
            //Debug.Log("Playing sound! Distance: ");
        }
        else
        {
            //Debug.Log("NOT playing - PlayerNearby: " + IsPlayerNearby());
        }
    }

}