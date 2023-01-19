using UnityEngine;

public class CardSpecifications : MonoBehaviour
{
    GameManager gameManager;
    CardManager cardManager;
    CardSpecifications cardSpesifications;
    Movement movementCode;
    Player player;

    public bool canSelectSinners, canSelectDevil, canSelectPlatforms;
    void Start()
    {
        cardManager = GameObject.Find("CARD MANAGER").GetComponent<CardManager>();
        cardSpesifications = GameObject.Find("CARD MANAGER").GetComponent<CardSpecifications>();
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        movementCode = GameObject.Find("GAME MANAGER").GetComponent<Movement>();
    }
    private void Update()
    {
        player = gameManager.player.GetComponent<Player>();
    }

    //COMMON
    public void Deli()
    {
        if(!gameManager.player.GetComponent<Player>().isBackEmpty && gameManager.player.GetComponent<Player>().canMoveForward)
        {
            player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 6);
        } else if(!gameManager.player.GetComponent<Player>().isLeftEmpty && gameManager.player.GetComponent<Player>().canMoveRight)
        {
            player.transform.position = new Vector3(player.transform.position.x - 6, player.transform.position.y, player.transform.position.z);
        } else if (!gameManager.player.GetComponent<Player>().isRightEmpty && gameManager.player.GetComponent<Player>().canMoveLeft)
        {
            player.transform.position = new Vector3(player.transform.position.x + 6, player.transform.position.y, player.transform.position.z);
        }
        gameManager.nextTurn = true;
    }
    public void DostYardimi()
    {
        canSelectSinners = true;
        movementCode.usedPower = "Dost Yard?m?";
    }
    public void Gezgin()
    {
        movementCode.canMove = true;
        movementCode.canMoveForward = true;
        movementCode.canMoveBackward = true;
        movementCode.canMoveRight = true;
        movementCode.canMoveLeft = true;
        gameManager.player.transform.Find("Arrows").gameObject.SetActive(true);
    }
    public void Pas()
    {
        gameManager.nextTurn = true;
    }

    public bool cardDestroyModeOpen = false;
    public void Ogutucu()
    {
        cardDestroyModeOpen = true;
    }

    //UNCOMMON
    public void Engelleme()
    {
        canSelectDevil = true;
        canSelectSinners = true;
        movementCode.usedPower = "Engelleme";
    }
    public void Celme()
    {
        canSelectSinners = true;
        movementCode.usedPower = "Çelme";
    }

    //RARE
    public void Durdurulamaz()
    {
        player.remainRoundsToLoseShield = 2;
        movementCode.NextTurn(1.5f);
    }
    public void Kalkan()
    {
        player.remainRoundsToLoseShield = 3;
        movementCode.NextTurn(1.5f);
    }
    public bool canSinnerTeleport;
    public void PortalGezgini()
    {
        canSinnerTeleport = true;
    }

    //EPIC
    public void RakipOgutucu()
    {
        canSelectSinners = true;
        canSelectDevil = true;
        movementCode.usedPower = "Rakip Ö?ütücü";
    }
    public void ZincirTuzagi()
    {
        movementCode.usedPower = "Zincir Tuza??";
        canSelectPlatforms = true;
    }
    public bool isDevilDogActive;
    public void SeytaninKopegi()
    {
        isDevilDogActive = false;
    }

    //LEGENDARY
    public void Canlandir()
    {
        canSelectSinners = true;
        movementCode.usedPower = "Canland?r";
    }
}
