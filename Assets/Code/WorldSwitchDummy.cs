using UnityEngine;

public class WorldSwitchDummy : MonoBehaviour
{
    [SerializeField] AudioClip swanMusic;
    [SerializeField] AudioClip ravenMusic;

    private bool isSwanWorld = true;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleWorld();
        }
    }

    void ToggleWorld()
    {
        isSwanWorld = !isSwanWorld;

        if (isSwanWorld)
        {
            Debug.Log("Switched to Swan World");
            GameManager.Instance.SwitchWorld(swanMusic);
        }
        else
        {
            Debug.Log("Switched to Raven World");
            GameManager.Instance.SwitchWorld(ravenMusic);
        }
    }
}