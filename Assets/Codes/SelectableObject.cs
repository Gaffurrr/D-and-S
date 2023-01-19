using UnityEngine;
using System.Collections;

public class SelectableObject : MonoBehaviour
{
    private Renderer renderer;

    CardManager cardManager;
    GameManager gameManager;
    Movement movementCode;
    Camera cam;

    public bool rotateForward, rotateBackward, rotateRight, rotateLeft;
    public bool isBackEmpty, isLeftEmpty, isRightEmpty;

    [Header("Queue System")]
    public Transform playerPos;
    public bool isFull;

    [Header("Teleport Points")]
    public bool isUnlocked;
    bool isUnlockedCounted;

    [SerializeField] AudioSource highlightSFX;
    void Start()
    {
        renderer = GetComponent<Renderer>();
        cardManager = GameObject.Find("CARD MANAGER").GetComponent<CardManager>();
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        movementCode = GameObject.Find("GAME MANAGER").GetComponent<Movement>();

        cam = GameObject.Find("CAMERA").GetComponent<Camera>();
    }
    private void Update()
    {
        if ((gameObject.tag == "Platform") || (gameObject.tag == "TeleportPoint" && isUnlocked))
        {
            PlatformSlot slot1 = transform.Find("PlayerPositions").transform.Find("Slot1").GetComponent<PlatformSlot>();
            PlatformSlot slot2 = transform.Find("PlayerPositions").transform.Find("Slot2").GetComponent<PlatformSlot>();
            PlatformSlot slot3 = transform.Find("PlayerPositions").transform.Find("Slot3").GetComponent<PlatformSlot>();
            PlatformSlot slot4 = transform.Find("PlayerPositions").transform.Find("Slot4").GetComponent<PlatformSlot>();
            if (slot1.isFull && slot2.isFull && slot3.isFull && slot4.isFull) { isFull = true; } else
            { 
                isFull = false; 
                if(!slot1.isFull)
                {
                    StartCoroutine(ChangePlatformSlot("Slot1"));
                } else if(!slot2.isFull)
                {
                    StartCoroutine(ChangePlatformSlot("Slot2"));
                } else if(!slot3.isFull)
                {
                    StartCoroutine(ChangePlatformSlot("Slot3"));
                } else if(!slot4.isFull)
                {
                    StartCoroutine(ChangePlatformSlot("Slot4"));
                }
            }
        }

        if (gameObject.tag == "TeleportPoint")
        {
            if (gameManager.player.transform.position.z >= transform.position.z)
            {
                isUnlocked = true;
            }
            if (!isUnlockedCounted)
            {
                gameManager.usableTeleportPointsCount++;
                isUnlockedCounted = true;
            }
            if(!isUnlocked)
            {
                transform.Find("Effect").gameObject.SetActive(false);
            } else transform.Find("Effect").gameObject.SetActive(true);

        }
    }
    IEnumerator ChangePlatformSlot(string slotName)
    {
        yield return new WaitForSeconds(1f);
        PlatformSlot slot1 = transform.Find("PlayerPositions").transform.Find("Slot1").GetComponent<PlatformSlot>();
        PlatformSlot slot2 = transform.Find("PlayerPositions").transform.Find("Slot2").GetComponent<PlatformSlot>();
        PlatformSlot slot3 = transform.Find("PlayerPositions").transform.Find("Slot3").GetComponent<PlatformSlot>();
        PlatformSlot slot4 = transform.Find("PlayerPositions").transform.Find("Slot4").GetComponent<PlatformSlot>();
        switch (slotName)
        {
            case "Slot1": playerPos = slot1.transform; break;
            case "Slot2": playerPos = slot2.transform; break;
            case "Slot3": playerPos = slot3.transform; break;
            case "Slot4": playerPos = slot4.transform; break;
        }
    }

    private void OnMouseEnter()
    {
        float distanceToMove;
        if(cardManager.cardPanel.activeSelf == false)
        {
            if (movementCode.canMove && cardManager.cardPanel.activeSelf == false)
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    SelectableObject selectableObject = hitInfo.collider.gameObject.GetComponent<SelectableObject>();
                    distanceToMove = Vector3.Distance(gameManager.player.transform.position, hitInfo.collider.transform.position);
                    if (selectableObject.transform.Find("HighlightState").gameObject.activeSelf) highlightSFX.Play();
                    //Check if selectable cube is a platform
                    if ((selectableObject.name == "EmptyCube" || selectableObject.name == "CardCube" || selectableObject.name == "Card+Cube") && (!selectableObject.isFull))
                    {
                        //Check if player can go forward
                        if (movementCode.canMoveForward)
                        {
                            if (distanceToMove < 13 && (selectableObject.transform.position.x < gameManager.player.transform.position.x + 3) && (selectableObject.transform.position.x > gameManager.player.transform.position.x - 3) && (selectableObject.transform.position.z > gameManager.player.transform.position.z + 3))
                            {
                                if (selectableObject != null)
                                {
                                    selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                }
                            }
                        } //Check if player can go backward
                        if (movementCode.canMoveBackward)
                        {
                            if (distanceToMove < 13 && (selectableObject.transform.position.x < gameManager.player.transform.position.x + 3) && (selectableObject.transform.position.x > gameManager.player.transform.position.x - 3) && (selectableObject.transform.position.z < gameManager.player.transform.position.z - 3))
                            {
                                if (selectableObject != null)
                                {
                                    selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                }
                            }
                        } //Check if player can go right
                        if (movementCode.canMoveRight)
                        {
                            if (distanceToMove < 13 && (selectableObject.transform.position.z < gameManager.player.transform.position.z + 3) && (selectableObject.transform.position.z > gameManager.player.transform.position.z - 3) && (selectableObject.transform.position.x > gameManager.player.transform.position.x + 3))
                            {
                                if (selectableObject != null)
                                {
                                    selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                }
                            }
                        } //Check if player can go left
                        if (movementCode.canMoveLeft)
                        {
                            if (distanceToMove < 13 && (selectableObject.transform.position.z < gameManager.player.transform.position.z + 3) && (selectableObject.transform.position.z > gameManager.player.transform.position.z - 3) && (selectableObject.transform.position.x < gameManager.player.transform.position.x - 3))
                            {
                                if (selectableObject != null)
                                {
                                    selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                }
                            }
                        }
                    }

                    //Check if selectable cube is a portal
                    if (selectableObject.name == "Portal" && gameManager.player.name != "Devil")
                    {
                        if (distanceToMove < 12)
                        {
                            if (selectableObject != null)
                            {
                                renderer.material.color = Color.red;
                            }               
                        }           
                    }                                                                                 
                                                                                                                                                                                                                                                           
                    if ((selectableObject.name == "Sinner1" || selectableObject.name == "Sinner2" || selectableObject.name == "Sinner3") && gameManager.player.GetComponent<Player>().isDevil)
                    {
                        if (distanceToMove < 10)
                        {
                            if (selectableObject != null)
                            {
                                selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                selectableObject.transform.Find("Trident").gameObject.SetActive(true);
                            }
                        }
                    }

                    //FOR CARD POWERS
                    if (cardManager.GetComponent<CardSpecifications>().canSelectSinners)
                    {
                        if ((selectableObject.name == "Sinner1" || selectableObject.name == "Sinner2" || selectableObject.name == "Sinner3") && selectableObject.GetComponent<Player>().remainRoundsToLoseShield == 0)
                        {
                            if (selectableObject != null)
                            {
                                switch (movementCode.usedPower)
                                {
                                    case "Engelleme":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true); break;
                                    case "Dost Yardýmý":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                        break;
                                    case "Çelme":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                        break;
                                    case "Rakip Öðütücü":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                        break;
                                    case "Canlandýr":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                        break;
                                }
                            }
                        }
                    }
                    else if ((cardManager.GetComponent<CardSpecifications>().canSelectDevil) && selectableObject.GetComponent<Player>().remainRoundsToLoseShield == 0)
                    {
                        if ((selectableObject.name == "Devil"))
                        {
                            if (selectableObject != null)
                            {
                                switch (movementCode.usedPower)
                                {
                                    case "Engelleme":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                        break;
                                    case "Rakip Öðütücü":
                                        selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                        break;
                                }
                            }
                        }
                    }

                    //Check if selectable object is a platform
                    if (cardManager.GetComponent<CardSpecifications>().canSelectPlatforms && selectableObject.tag == "Platform")
                    {
                        if (selectableObject != null)
                        {
                            switch (movementCode.usedPower)
                            {
                                case "Zincir Tuzaðý":
                                    selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                                    break;
                            }
                        }
                    }

                    //Check if selectable cube is a teleport point
                    if (selectableObject.tag == "TeleportPoint")
                    {
                        if (gameManager.player.name == "Devil")
                        {
                            if (gameManager.player.GetComponent<DevilSpesifications>().isUltiUsable && selectableObject.isUnlocked)
                            {
                                selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                            }
                        }
                        if (selectableObject.isUnlocked && cardManager.GetComponent<CardSpecifications>().canSinnerTeleport)
                        {
                            selectableObject.transform.Find("HighlightState").gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    private void OnMouseExit()
    {
        if(gameObject.name == "EmptyCube" || gameObject.name == "CardCube" || gameObject.name == "Card+Cube" || gameObject.tag == "TeleportPoint" || gameObject.tag == "Player")
        {
            transform.Find("HighlightState").gameObject.SetActive(false);
        }
        if(gameObject.tag == "Player")
        {
            transform.Find("Trident").gameObject.SetActive(false);
        }
    }
}
