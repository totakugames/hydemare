using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject CreditsMenu;

    public void OnBtnPlay()
    {
        SceneManager.LoadScene("finalLevel");
    }

    public void OnBtnCredits()
    {
        MainMenu.SetActive(false);
        CreditsMenu.SetActive(true);
    }

    public void OnBtnQuit()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void OnBtnBack() 
    {
        CreditsMenu.SetActive(false);
        MainMenu.SetActive(true);
    }
}
