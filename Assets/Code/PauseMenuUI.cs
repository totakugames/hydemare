using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MenuCanvas;
    
    private InputAction Menu;

    private float PressedCooldown = 0.5f;
    private float LastPressedTime;

    void Start() 
    {
        Menu = InputSystem.actions.FindAction("Menu");
        LastPressedTime = Time.realtimeSinceStartup;
    }

    public void OnPause() 
    {
        float tSincePress = Time.realtimeSinceStartup - LastPressedTime;
        if (tSincePress > PressedCooldown) 
        {
            MenuCanvas.SetActive(!MenuCanvas.activeSelf);
            Time.timeScale = Time.timeScale == 0 ? 1 : 0;
            LastPressedTime = Time.realtimeSinceStartup;
        }
    }

    public void OnBtnContinue()
    {
        MenuCanvas.SetActive(!MenuCanvas.activeSelf);
        Time.timeScale = Time.timeScale == 0 ? 1 : 0;
    }

    public void OnBtnToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }

    public void OnSliderVolume(float value)
    {
        AudioListener.volume = value;
    }
}
