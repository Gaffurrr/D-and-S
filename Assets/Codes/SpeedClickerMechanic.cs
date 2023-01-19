using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpeedClickerMechanic : MonoBehaviour
{
    [SerializeField] GameObject buttonPrefab;
    GameObject currentButton;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] int score;

    [SerializeField] Transform[] spawnLocations;

    void Start()
    {
        SpawnButton();
    }

    private void Update()
    {
        scoreText.text = score.ToString();
    }

    void SpawnButton()
    {
        int i = Random.Range(0, spawnLocations.Length);
        currentButton = Instantiate(buttonPrefab, spawnLocations[i].position, Quaternion.identity);
        currentButton.transform.parent = transform;
        currentButton.GetComponent<Button>().onClick.AddListener(DestroyButton);
    }
    public void DestroyButton()
    {
        Destroy(currentButton);
        score += 1;
        SpawnButton();
    }
}
