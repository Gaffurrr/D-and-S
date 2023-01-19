using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] Camera cam;
    public GameObject player;
    CardManager cardManager;
    GameManager gameManager;

    public float distanceToMove;
    float maxDistanceToMove, maxDistanceToCatch;
    public bool canMoveBackward, canMoveForward, canMoveLeft, canMoveRight, canMove;
    RaycastHit mainHitInfo;
    Transform targetPlatform;

    bool playMovementAnimation = false;
    bool playTeleportMovement = false;
    SelectableObject selectedObject;

    [Header("Powers")]
    public string usedPower;
    public GameObject trapPrefab;
    void Start()
    {
        canMoveForward = true;
        canMove = true;
        maxDistanceToMove = 13f;
        maxDistanceToCatch = 10f;

        cardManager = GameObject.Find("CARD MANAGER").GetComponent<CardManager>();
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
    }

    void Update()
    {
        player = gameManager.player;

        if (Input.GetMouseButtonDown(0) && canMove && cardManager.cardPanel.activeSelf == false)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                mainHitInfo = hitInfo;
                SelectableObject selectableObject = hitInfo.collider.gameObject.GetComponent<SelectableObject>();
                selectedObject = selectableObject;
                distanceToMove = Vector3.Distance(player.transform.position, hitInfo.collider.transform.position);

                //Check if selectable cube is a platform
                if ((selectableObject.name == "EmptyCube" || selectableObject.name == "CardCube" || selectableObject.name == "Card+Cube") && (!selectableObject.isFull))
                {
                    //Check if player can go forward
                    if (canMoveForward)
                    {
                        if (distanceToMove < maxDistanceToMove && (selectableObject.transform.position.x < player.transform.position.x + 3) && (selectableObject.transform.position.x > player.transform.position.x - 3) && (selectableObject.transform.position.z > player.transform.position.z + 4))
                        {
                            if (selectableObject != null)
                            {
                                playMovementAnimation = true;
                                targetPlatform = mainHitInfo.transform;
                            }
                        }
                    } //Check if player can go backward
                    if (canMoveBackward)
                    {
                        if (distanceToMove < maxDistanceToMove && (selectableObject.transform.position.x < player.transform.position.x + 3) && (selectableObject.transform.position.x > player.transform.position.x - 3) && (selectableObject.transform.position.z < player.transform.position.z - 4))
                        {
                            if (selectableObject != null)
                            {
                                playMovementAnimation = true;
                                targetPlatform = mainHitInfo.transform;
                            }
                        }
                    } //Check if player can go right
                    if (canMoveRight)
                    {
                        if (distanceToMove < maxDistanceToMove && (selectableObject.transform.position.z < player.transform.position.z + 3) && (selectableObject.transform.position.z > player.transform.position.z - 3) && (selectableObject.transform.position.x > player.transform.position.x + 4))
                        {
                            if (selectableObject != null)
                            {
                                playMovementAnimation = true;
                                targetPlatform = mainHitInfo.transform;
                            }
                        }
                    } //Check if player can go left
                    if (canMoveLeft)
                    {
                        if (distanceToMove < maxDistanceToMove && (selectableObject.transform.position.z < player.transform.position.z + 3) && (selectableObject.transform.position.z > player.transform.position.z - 3) && (selectableObject.transform.position.x < player.transform.position.x - 4))
                        {
                            if (selectableObject != null)
                            {
                                playMovementAnimation = true;
                                targetPlatform = mainHitInfo.transform;
                            }
                        }
                    }
                }

                //Check if selectable cube is a portal
                if (selectableObject.name == "Portal" && gameManager.player.name != "Devil")
                {
                    if (distanceToMove < 12)
                    {
                        if(selectableObject != null)
                        {
                            gameManager.FinishGame(false);
                            StartCoroutine(NextTurn(2.5f));
                        }
                    }
                }

                //Check if selectable player is playing as a sinner and the current player is devil
                if ((selectableObject.name == "Sinner1" || selectableObject.name == "Sinner2" || selectableObject.name == "Sinner3") && player.GetComponent<Player>().isDevil && selectableObject.GetComponent<Player>().remainRoundsToLoseShield == 0 && !selectableObject.GetComponent<Player>().isDead)
                {
                    if (distanceToMove < maxDistanceToCatch)
                    {
                        if (selectableObject != null)
                        {
                            selectableObject.GetComponent<Player>().isDead = true;
                            GameObject.Find("UI").transform.Find("BigAnnouncement").transform.Find("KillAnnouncement").gameObject.SetActive(true);
                            GameObject.Find("UI").transform.Find("BigAnnouncement").transform.Find("KillAnnouncement").transform.Find("Texts").transform.Find("PlayerNick").GetComponent<TMPro.TMP_Text>().text = selectableObject.GetComponent<Player>().playersNick;
                            StartCoroutine(NextTurnAfterKilled(1.5f));
                        }
                    }
                }

                //Check if selectable cube is a teleport point
                if (selectableObject.tag == "TeleportPoint")
                {
                    if (gameManager.player.GetComponent<Player>().isDevil)
                    {
                        if (gameManager.player.GetComponent<DevilSpesifications>().isUltiUsable && selectableObject.isUnlocked)
                        {
                            playMovementAnimation = true;
                            gameManager.player.GetComponent<DevilSpesifications>().isUltiUsed = true;
                            StartCoroutine(NextTurn(3.5f));
                        }
                    }
                    else if (gameManager.player.GetComponent<Player>().isSinner && selectableObject.isUnlocked && cardManager.GetComponent<CardSpecifications>().canSinnerTeleport)
                    {
                        playMovementAnimation = true;
                        StartCoroutine(NextTurn(2.5f));
                    }
                }

                //FOR CARD POWERS
                //Check if selectable object is a player
                if (cardManager.GetComponent<CardSpecifications>().canSelectSinners)
                {
                    if ((selectableObject.name == "Sinner1" || selectableObject.name == "Sinner2" || selectableObject.name == "Sinner3") && selectableObject.GetComponent<Player>().remainRoundsToLoseShield == 0)
                    {
                        if (selectableObject != null)
                        {
                            switch(usedPower)
                            {
                                case "Engelleme":
                                   selectableObject.GetComponent<Player>().remainRoundsToPlay = 2;
                                    StartCoroutine(NextTurn(0.5f)); break;
                                case "Dost Yard?m?":
                                    if (selectableObject.GetComponent<Player>().canMoveForward)
                                    {
                                        selectableObject.transform.position = new Vector3(selectableObject.transform.position.x, selectableObject.transform.position.y, selectableObject.transform.position.z + 6);
                                    }
                                    else if (selectableObject.GetComponent<Player>().canMoveRight)
                                    {
                                        selectableObject.transform.position = new Vector3(selectableObject.transform.position.x + 6, selectableObject.transform.position.y, selectableObject.transform.position.z);
                                    }
                                    else if (selectableObject.GetComponent<Player>().canMoveLeft)
                                    {
                                        selectableObject.transform.position = new Vector3(selectableObject.transform.position.x - 6, selectableObject.transform.position.y, selectableObject.transform.position.z);
                                    }
                                    cardManager.canOpenCard = false;
                                    cardManager.isSinnerChoosing = false;
                                    gameManager.nextTurn = true;
                                    break;
                                case "Çelme":
                                    if (!selectableObject.GetComponent<Player>().isBackEmpty)
                                    {
                                        selectableObject.transform.position = new Vector3(selectableObject.transform.position.x, selectableObject.transform.position.y, selectableObject.transform.position.z - 6);
                                    }
                                    else if (!selectableObject.GetComponent<Player>().isRightEmpty)
                                    {
                                        selectableObject.transform.position = new Vector3(selectableObject.transform.position.x - 6, selectableObject.transform.position.y, selectableObject.transform.position.z);
                                    }
                                    else if (!selectableObject.GetComponent<Player>().isLeftEmpty)
                                    {
                                        selectableObject.transform.position = new Vector3(selectableObject.transform.position.x + 6, selectableObject.transform.position.y, selectableObject.transform.position.z);
                                    }
                                    cardManager.canOpenCard = false;
                                    cardManager.isSinnerChoosing = false;
                                    gameManager.nextTurn = true;
                                    break;
                                case "Rakip Ö?ütücü":
                                    if(GameObject.Find("UI").transform.Find(gameManager.player.name).GetComponent<InventorySystem>().cardCount > 0)
                                    {
                                        cardManager.GetComponent<CardSpecifications>().cardDestroyModeOpen = true;
                                        GameObject.Find("UI").transform.Find(gameManager.player.name).gameObject.SetActive(false);
                                        GameObject.Find("UI").transform.Find(selectableObject.name).gameObject.SetActive(true);
                                    } else
                                    {
                                        StartCoroutine(NextTurn(1.5f));
                                    }
                                    break;
                                case "Canland?r": if(selectableObject.GetComponent<Player>().isDead)
                                    {
                                        selectableObject.GetComponent<Player>().isDead = false;
                                        selectableObject.GetComponent<Player>().willResurrect = true;
                                        selectableObject.GetComponent<Player>().transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z);
                                        StartCoroutine(NextTurn(1.5f));
                                    }
                                    break;
                            }
                            cardManager.GetComponent<CardSpecifications>().canSelectSinners = false;
                            cardManager.GetComponent<CardSpecifications>().canSelectDevil = false;
                            canMove = false;
                        }
                    }
                } else if((cardManager.GetComponent<CardSpecifications>().canSelectDevil) && selectableObject.GetComponent<Player>().remainRoundsToLoseShield == 0)
                {
                    if ((selectableObject.name == "Devil"))
                    {
                        if (selectableObject != null)
                        {
                            switch (usedPower)
                            {
                                case "Engelleme":
                                    selectableObject.GetComponent<Player>().remainRoundsToPlay = 4;
                                    StartCoroutine(NextTurn(0.5f));
                                    break;
                                case "Rakip Ö?ütücü":
                                    cardManager.GetComponent<CardSpecifications>().cardDestroyModeOpen = true;
                                    GameObject.Find("UI").transform.Find(gameManager.player.name).gameObject.SetActive(false);
                                    GameObject.Find("UI").transform.Find(selectableObject.name).gameObject.SetActive(true);
                                    break;
                            }
                            canMove = false;
                            cardManager.GetComponent<CardSpecifications>().canSelectDevil = false;
                            cardManager.GetComponent<CardSpecifications>().canSelectSinners = false;
                        }
                    }
                }

                //Check if selectable object is a platform
                if(cardManager.GetComponent<CardSpecifications>().canSelectPlatforms && selectableObject.tag == "Platform")
                {
                    if(selectableObject != null)
                    {
                        switch (usedPower)
                        {
                            case "Zincir Tuza??":
                                selectableObject.transform.Find("ChainTrap").gameObject.SetActive(true);
                                StartCoroutine(NextTurn(1.5f));
                                break;
                        }
                        canMove = false;
                        cardManager.GetComponent<CardSpecifications>().canSelectPlatforms = false;
                    }
                }

                //FOR NEXT TURN SYSTEM
                if (selectableObject.name == "EmptyCube") StartCoroutine(NextTurn(2.5f));
            }
        }
        if (playMovementAnimation) StartCoroutine(MovementAnimation());
    }

    IEnumerator MovementAnimation()
    {
        canMove = false;
        if(player.transform.position.y > 0f)
        {
            if (gameManager.player.transform.Find("Arrows").gameObject.activeSelf)
            {
                gameManager.player.transform.Find("Arrows").gameObject.SetActive(false);
            }
            player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(player.transform.position.x, mainHitInfo.transform.position.y + 4f, player.transform.position.z), 10f * Time.deltaTime);

            if (player.transform.position == new Vector3(player.transform.position.x, mainHitInfo.transform.position.y + 4f, player.transform.position.z))
            {
                yield return new WaitForSeconds(0.4f);

                player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(selectedObject.playerPos.position.x, player.transform.position.y, selectedObject.playerPos.position.z), 20f * Time.deltaTime);

                if (player.transform.position == new Vector3(selectedObject.playerPos.position.x, player.transform.position.y, selectedObject.playerPos.position.z))
                {
                    yield return new WaitForSeconds(0.2f);

                    player.transform.position = Vector3.MoveTowards(player.transform.position, new Vector3(selectedObject.playerPos.position.x, mainHitInfo.transform.position.y + 2f, selectedObject.playerPos.position.z), 10f * Time.deltaTime);

                    yield return new WaitForSeconds(0.4f);

                    playMovementAnimation = false;
                }
            }
        }
    }

    public IEnumerator NextTurn(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        gameManager.nextTurn = true;
    }
    public IEnumerator NextTurnAfterKilled(float seconds)
    {
        yield return new WaitForSeconds(2.5f);
        GameObject.Find("UI").transform.Find("BigAnnouncement").transform.Find("KillAnnouncement").gameObject.SetActive(false);
        yield return new WaitForSeconds(seconds);
        gameManager.nextTurn = true;
    }
}
