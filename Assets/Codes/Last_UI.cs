using UnityEngine;

public class Last_UI : MonoBehaviour
{
    int deadPlayerCount;
    [SerializeField] TMPro.TMP_Text deadPlayerCountText;

    private void Start()
    {
        deadPlayerCount = PlayerPrefs.GetInt("DeadSinnerCount");
        deadPlayerCountText.text = deadPlayerCount.ToString();
    }
    public void ReturnToMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
