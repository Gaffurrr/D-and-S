using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu, settingsPage;
    [SerializeField] UnityEngine.UI.Toggle fullScreen;
    [SerializeField] UnityEngine.UI.Slider musicSlider;
    [SerializeField] AudioSource[] musics;

    void Start()
    {
        Time.timeScale = 1;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale != 0)
        {
            StopGame();
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 0)
        {
            if(settingsPage.activeSelf)
            {
                TurnMain();
            } else
            {
                Continue();
            }
        }

        foreach(AudioSource music in musics)
        {
            music.volume = musicSlider.value;
        }
    }
    void StopGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Continue()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }
    public void OpenSettings()
    {
        pauseMenu.SetActive(false);
        settingsPage.SetActive(true);
    }
    public void TurnMain()
    {
        pauseMenu.SetActive(true);
        settingsPage.SetActive(false);
    }
    public void ChangeScene(int sceneNumber)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneNumber);
    }

    //SETTINGS
    public void Fullscreen()
    {
        Screen.fullScreen = fullScreen.isOn;
    }
}
