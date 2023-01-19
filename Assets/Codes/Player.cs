using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isSinner, isDevil;
    GameObject sign;

    CardManager cardManager;
    GameManager gameManager;
    Movement movementCode;
    GameObject userInterface;

    public bool isDead, isRunAway, willResurrect;
    bool deathCounted, runCounted;

    public int remainRoundsToLoseShield;
    public int remainRoundsToPlay;
    bool didRemainInts;

    int currentLevelNumber;
    public string playersNick;
    public bool canMoveForward, canMoveBackward, canMoveRight, canMoveLeft;
    public bool isBackEmpty, isLeftEmpty, isRightEmpty;

    [Header("UI Part")]
    TMPro.TMP_Text powerTitle, remainingRoundsText;

    [Header("Level System")]
    int randomLevel;

    [SerializeField] AudioSource runAwaySFX;

    private void Start()
    {
        cardManager = GameObject.Find("CARD MANAGER").GetComponent<CardManager>();
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        movementCode = GameObject.Find("GAME MANAGER").GetComponent<Movement>();
        userInterface = GameObject.Find("UI");
        sign = transform.Find("Sign").gameObject;
        remainRoundsToPlay = 0;
        remainRoundsToLoseShield = 0;
        currentLevelNumber = 0;

        randomLevel = Random.Range(1, 2);

        switch (gameObject.name)
        {
            case "Sinner1": playersNick = PlayerPrefs.GetString("Sinner1Name"); gameManager.queueIcons[0].SetActive(true); gameManager.queueIcons[3].SetActive(false); break;
            case "Sinner2": playersNick = PlayerPrefs.GetString("Sinner2Name"); gameManager.queueIcons[1].SetActive(true); gameManager.queueIcons[3].SetActive(false); break;
            case "Sinner3": playersNick = PlayerPrefs.GetString("Sinner3Name"); gameManager.queueIcons[2].SetActive(true); gameManager.queueIcons[3].SetActive(false); break;
            case "Devil": playersNick = PlayerPrefs.GetString("DevilName"); gameManager.queueIcons[3].SetActive(true); gameManager.queueIcons[0].SetActive(false); gameManager.queueIcons[1].SetActive(false); gameManager.queueIcons[2].SetActive(false); break;
        }
        powerTitle = userInterface.transform.Find(gameObject.name).transform.Find("PowerWarning").transform.Find("Power").GetComponent<TMPro.TMP_Text>();
        remainingRoundsText = userInterface.transform.Find(gameObject.name).transform.Find("PowerWarning").transform.Find("RemainingRound").GetComponent<TMPro.TMP_Text>();
    }
    private void Update()
    {
        remainRoundsToPlay = Mathf.Clamp(remainRoundsToPlay, 0, 10);
        remainRoundsToLoseShield = Mathf.Clamp(remainRoundsToLoseShield, 0, 4);

        canMoveForward = movementCode.canMoveForward; canMoveBackward = movementCode.canMoveBackward;
        canMoveRight = movementCode.canMoveRight; canMoveLeft = movementCode.canMoveLeft;
        if (gameManager.nextTurn) GetComponent<Player>().enabled = false;
        if (gameManager.player != gameObject)
        {
            GetComponent<Player>().enabled = false;
            didRemainInts = false;
        }
        else
        {
            if(!didRemainInts)
            {
                remainRoundsToPlay--; remainRoundsToLoseShield--;
                didRemainInts = true;
            }
            if (willResurrect)
            {
                transform.Find("Mesh").gameObject.SetActive(true);
                transform.Find("StoneBlock").gameObject.SetActive(true);
                transform.Find("DeadEffect").gameObject.SetActive(false);
                gameManager.deadPlayerCount--;
                willResurrect = false;
            }
        }

        if(remainRoundsToPlay > 0)
        {
            movementCode.canMove = false;
            userInterface.transform.Find(gameObject.name).transform.Find("PowerWarning").gameObject.SetActive(true);
            powerTitle.text = "Engel - ";
            remainingRoundsText.text = remainRoundsToPlay.ToString() + " tur kald?";
            Invoke("NextTurn", 2.5f);
        }
        else if (remainRoundsToLoseShield > 0)
        {
            userInterface.transform.Find(gameObject.name).transform.Find("PowerWarning").gameObject.SetActive(true);
            powerTitle.text = "Kalkan - ";
            remainingRoundsText.text = remainRoundsToLoseShield.ToString() + " tur kald?";
            transform.Find("Shield").gameObject.SetActive(true);
        } else { transform.Find("Shield").gameObject.SetActive(false); userInterface.transform.Find(gameObject.name).transform.Find("PowerWarning").gameObject.SetActive(false); }
        if(isDead && !deathCounted)
        {
            if (cardManager.GetComponent<CardSpecifications>().isDevilDogActive)
            {
                isDevil = true; isSinner = false; isDead = false; transform.Find("Mesh").gameObject.SetActive(true); transform.Find("DeadEffect").gameObject.SetActive(false);
            } else
            {
                isDead = true;
                gameManager.deadPlayerCount++;
                deathCounted = true;
                transform.Find("Mesh").gameObject.SetActive(false);
                transform.Find("StoneBlock").gameObject.SetActive(true);
                transform.Find("DeadEffect").gameObject.SetActive(true);

            }
        } else if(isRunAway && !runCounted)
        {
            gameManager.escapedSinnerCount++;
            runCounted = true;
            runAwaySFX.Play();
            GameObject.Find("UI").transform.Find("BigAnnouncement").transform.Find("EscapeAnnouncement").gameObject.SetActive(true);
            GameObject.Find("UI").transform.Find("BigAnnouncement").transform.Find("EscapeAnnouncement").transform.Find("Texts").transform.Find("PlayerNick").GetComponent<TMPro.TMP_Text>().text = playersNick;
        } else if(isDead && deathCounted && !cardManager.GetComponent<CardSpecifications>().isDevilDogActive)
        {
            isDevil = false; isSinner = true;
        }
        if(!cardManager.GetComponent<CardSpecifications>().cardDestroyModeOpen)
        {
            switch (gameManager.player.name)
            {
                case "Sinner1": userInterface.transform.Find("Sinner1").gameObject.SetActive(true); userInterface.transform.Find("Devil").gameObject.SetActive(false); break;
                case "Sinner2": userInterface.transform.Find("Sinner2").gameObject.SetActive(true); userInterface.transform.Find("Devil").gameObject.SetActive(false); break;
                case "Sinner3": userInterface.transform.Find("Sinner3").gameObject.SetActive(true); userInterface.transform.Find("Devil").gameObject.SetActive(false); break;
                case "Devil": userInterface.transform.Find("Devil").gameObject.SetActive(true); userInterface.transform.Find("Sinner1").gameObject.SetActive(false); userInterface.transform.Find("Sinner2").gameObject.SetActive(false); userInterface.transform.Find("Sinner3").gameObject.SetActive(false); break;
            }
        }
        if (gameManager.players[gameManager.currentPlayer] == this) 
        {
            sign.SetActive(true);
        } else sign.SetActive(false);

        switch(currentLevelNumber)
        {
            case 0: GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(true); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(false); break;
            case 1: GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(true); break;
            case 2: GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(true); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(false); break;
            case 3: GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(true); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(false); break;
        }
    }
    public void NextLevel()
    {
        switch(currentLevelNumber)
        {
            case 0: switch(randomLevel)
                {
                    case 1: transform.position = GameObject.Find("ENVIRONMENT").transform.Find("DESERT").transform.Find("PLATFORMS").transform.Find("StartPoint").transform.position; currentLevelNumber = 1; break;
                    case 2: transform.position = GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD").transform.Find("PLATFORMS").transform.Find("StartPoint").transform.position; currentLevelNumber = 2; break;
                }
                break;
            case 1: transform.position = GameObject.Find("ENVIRONMENT").transform.Find("HELL_P2").transform.Find("PLATFORMS").transform.Find("StartPoint").transform.position; currentLevelNumber = 3; break;
            case 2: transform.position = GameObject.Find("ENVIRONMENT").transform.Find("HELL_P2").transform.Find("PLATFORMS").transform.Find("StartPoint").transform.position; currentLevelNumber = 3; break;
            case 3: isRunAway = true; break;
        }
    }
    void NextTurn()
    {
        gameManager.nextTurn = true;
    }
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "CardCube" || col.gameObject.name == "Card+Cube")
        {
            if (isSinner) { cardManager.isSinnerChoosing = true; cardManager.isDevilChoosing = false; }
            if (isDevil) { cardManager.isSinnerChoosing = false; cardManager.isDevilChoosing = true; }
            cardManager.isCardChoosing = true;
            cardManager.isCommonCards = true;
        }
        if (col.gameObject.name == "Card+Cube") { cardManager.isAllCards = true; cardManager.isCommonCards = false; }
        if(col.gameObject.tag == "TeleportPointLock")
        {
            col.transform.parent.GetComponent<SelectableObject>().isUnlocked = true;
        }
        if(col.gameObject.tag == "Trap")
        {
            remainRoundsToPlay = 1;
            col.gameObject.SetActive(false);
        }
        if(col.gameObject.tag == "Portal" && isSinner)
        {
            gameManager.FinishGame(false);
        }
    }
    public SelectableObject platform;
    private void OnTriggerStay(Collider col)
    {
        if ((col.gameObject.tag == "Platform" || col.gameObject.tag == "TeleportPoint") && gameObject == gameManager.player)
        {
            platform = col.GetComponent<SelectableObject>();
            
            if (platform.rotateForward) { movementCode.canMoveForward = true; } else { movementCode.canMoveForward = false; }
            if (platform.rotateBackward) { movementCode.canMoveBackward = true; } else { movementCode.canMoveBackward = false; }
            if (platform.rotateRight) { movementCode.canMoveRight = true; } else { movementCode.canMoveRight = false; }
            if (platform.rotateLeft) { movementCode.canMoveLeft = true; } else { movementCode.canMoveLeft = false; }
            if(platform.isBackEmpty) { isBackEmpty = true; } else { isBackEmpty = false; }
            if (platform.isRightEmpty) { isRightEmpty = true; } else { isRightEmpty = false; }
            if (platform.isLeftEmpty) { isLeftEmpty = true; } else { isLeftEmpty = false; }
        }

        if(col.gameObject.name == "Hell")
        {
            GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(true); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(false);
        }
        else if(col.gameObject.name == "Desert")
        {
            GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(true);
        }
        else if(col.gameObject.name == "IceWorld")
        {
            GameObject.Find("ENVIRONMENT").transform.Find("HELL_GLOBAL_VOLUME").gameObject.SetActive(false); GameObject.Find("ENVIRONMENT").transform.Find("ICE_WORLD_GLOBAL_VOLUME").gameObject.SetActive(true); GameObject.Find("ENVIRONMENT").transform.Find("DESERT_GLOBAL_VOLUME").gameObject.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider col)
    {
    }
}
