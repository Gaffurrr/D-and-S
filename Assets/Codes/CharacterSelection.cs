using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] TMP_InputField devilName, sinner1Name, sinner2Name, sinner3Name;
    public void ChangeScene(int sceneNumber)
    {
        PlayerPrefs.SetString("DevilName", devilName.text);
        PlayerPrefs.SetString("Sinner1Name", sinner1Name.text);
        PlayerPrefs.SetString("Sinner2Name", sinner2Name.text);
        PlayerPrefs.SetString("Sinner3Name", sinner3Name.text);

        SceneManager.LoadScene(sceneNumber);
    }
}
