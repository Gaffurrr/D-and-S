using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Toggle fullScreen;
    [SerializeField] GameObject[] pages;

    private void Start()
    {
        Time.timeScale = 1;
    }
    public void ChangeScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }
    public void OpenPage(int pageNo)
    {
        pages[pageNo].SetActive(true);
    }
    public void ClosePage(int pageNo)
    {
        pages[pageNo].SetActive(false);
    }
    public void Exit()
    {
        Application.Quit();
    }

    //SETTINGS
    public void Fullscreen()
    {
        Screen.fullScreen = fullScreen.isOn;
    }
}
