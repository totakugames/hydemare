using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject WinTxt;
    [SerializeField]
    private GameObject LossTxt;

    public void SetEnding(bool win)
    {
        WinTxt.SetActive(win);
        LossTxt.SetActive(!win);
        Time.timeScale = 0;
    }

    public void OnBtnToMenu()
    {
        SceneManager.LoadScene("mainMenu");
    }
}
