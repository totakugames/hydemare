using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private GameObject CreditsMenu;
    [SerializeField]
    private GameObject TutorialMenu;

    public void OnBtnPlay()
    {
        MainMenu.SetActive(false);
        TutorialMenu.SetActive(true);
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

    public void OnPlay()
    {
        SceneManager.LoadScene("finalLevel");
    }
}
