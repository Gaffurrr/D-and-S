using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Camera cam;
    [SerializeField] Vector3 camDistance;
    public float cameraSpeed = 13.5f;

    CardManager cardManager;
    public GameObject player;

    public int usableTeleportPointsCount = 0;


    [Header("Queue System")]
    public Player[] players;
    public int currentPlayer;
    int maxPlayerCount = 5;
    public bool nextTurn;
    public int deadPlayerCount;
    public int escapedSinnerCount;

    int nextDevil;

    [Header("UI Part")]
    [SerializeField] TMPro.TMP_Text queueText;
    public GameObject[] queueIcons;
    [SerializeField] GameObject queueAnnouncementPanel;
    [SerializeField] TMPro.TMP_Text queueAnnouncementText;

    void Start()
    {
        currentPlayer = 0;
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        cardManager = GameObject.Find("CARD MANAGER").GetComponent<CardManager>();
        maxPlayerCount = 5;
        deadPlayerCount = 0;
        nextDevil = -1;
    }
    void Update()
    {
        player = players[currentPlayer].gameObject;
        players[currentPlayer].enabled = true;

        queueText.text = player.GetComponent<Player>().playersNick;
        queueAnnouncementText.text = player.GetComponent<Player>().playersNick;
        switch (players[currentPlayer].name)
        {
            case "Sinner1": queueIcons[0].SetActive(true); queueIcons[3].SetActive(false); queueIcons[1].SetActive(false); queueIcons[2].SetActive(false); break;
            case "Sinner2": queueIcons[1].SetActive(true); queueIcons[3].SetActive(false);  queueIcons[0].SetActive(false); queueIcons[2].SetActive(false); break;
            case "Sinner3": queueIcons[2].SetActive(true); queueIcons[3].SetActive(false); queueIcons[1].SetActive(false); queueIcons[0].SetActive(false); break;
            case "Devil": queueIcons[3].SetActive(true); queueIcons[0].SetActive(false); queueIcons[1].SetActive(false); queueIcons[2].SetActive(false); break;
        }
        if (deadPlayerCount >= 3) FinishGame(true);
        if (escapedSinnerCount >= 1) FinishGame(false);
        if(nextTurn) StartCoroutine(NextTurn());

        if (players[currentPlayer].isRunAway)
        {
            nextTurn = true;
            nextDevil = currentPlayer + 1;
        }
        if(players[currentPlayer].isDead)
        {
            nextTurn = true;
            nextDevil = currentPlayer + 1;
        } else if(player.name == "Devil" && currentPlayer == nextDevil)
        {
            nextTurn = true;
        }
    }
    private void FixedUpdate()
    {
        if(Input.GetMouseButton(1))
        {
            cameraSpeed = 35f;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, player.transform.Find("TopDownPerspective").transform.position, cameraSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, player.transform.Find("TopDownPerspective").transform.rotation, 5f * Time.deltaTime);
        } else
        {
            float distanceBetweenPlayerAndCamera = Vector3.Distance(player.transform.Find("CameraPosition").transform.position, cam.transform.position);
            if (distanceBetweenPlayerAndCamera > 15f) { cameraSpeed = 50f; } else cameraSpeed = 12f;
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, player.transform.Find("CameraPosition").transform.position, cameraSpeed * Time.deltaTime);
            cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, player.transform.Find("CameraPosition").transform.rotation, 2.5f * Time.deltaTime);
        }
    }
    IEnumerator NextTurn()
    {
        if(!player.GetComponent<Player>().isDead && (currentPlayer != nextDevil))
        {
            queueAnnouncementPanel.SetActive(true);
            if (player.name != "Devil") GameObject.Find("PLAYERS").transform.Find("Devil").GetComponent<DevilSpesifications>().ultiValue += 20;
            if (currentPlayer >= maxPlayerCount) { currentPlayer = 0; } else { currentPlayer++; }
            nextTurn = false;
            yield return new WaitForSeconds(2.5f);
            GetComponent<Movement>().canMove = true;
            queueAnnouncementPanel.SetActive(false);
            cardManager.canOpenCard = true;
        } else
        {
            if (currentPlayer >= maxPlayerCount) { currentPlayer = 0; } else { currentPlayer++; }
            nextTurn = false;
            GetComponent<Movement>().canMove = true;
            cardManager.canOpenCard = true;
        }
    }
    public void FinishGame(bool isDevilWon)
    {
        if(isDevilWon)
        {
            SceneManager.LoadScene(3);
        } else
        {
            SceneManager.LoadScene(4);
            PlayerPrefs.SetInt("DeadSinnerCount", deadPlayerCount);
        }
    }
}
