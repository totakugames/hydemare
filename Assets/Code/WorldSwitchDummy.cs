using UnityEngine;
using UnityEngine.InputSystem;

public class WorldSwitchDummy : MonoBehaviour
{
    [SerializeField] AudioClip swanMusic;
    [SerializeField] AudioClip ravenMusic;

    private bool isSwanWorld = true;

    private InputAction DoMask;

    void Start()
    {
        DoMask = InputSystem.actions.FindAction("SwitchMask");
    }

    void Update()
    {
        //if (DoMask.IsPressed())
        //{
        //   ToggleWorld();
        //}
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