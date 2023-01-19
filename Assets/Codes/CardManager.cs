using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardManager : MonoBehaviour
{
    public bool isSinnerChoosing, isDevilChoosing;
    GameManager gameManager;
    GameObject userInterface;

    [Header("UI Part")]
    public GameObject cardPanel;
    //[SerializeField] GameObject currentCardPanel;
    //public TMPro.TMP_Text currentCard;

    [Header("Sinner Cards")]
    public Card[] sinnerCommonCards;
    public Card[] sinnerUncommonCards;
    public Card[] sinnerRareCards;
    public Card[] sinnerEpicCards;
    public Card[] sinnerLegendaryCards;

    [Header("Devil Cards")]
    public Card[] devilCommonCards;
    public Card[] devilUncommonCards;
    public Card[] devilRareCards;
    public Card[] devilEpicCards;
    public Card[] devilLegendaryCards;

    [Header("Card System")]
    [SerializeField] Transform[] slotPosition;
    float randomValue;
    public bool isCardChoosing;
    public bool isCommonCards, isAllCards;
    //public bool isCardUsing;
    public bool canOpenCard;

    [Header("Countdown System")]
    bool timerIsRunning = false;
    float timeRemaining = 15f;
    [SerializeField] Text countdownText;

    [Header("SFX")]
    [SerializeField] AudioSource cardUseSFX;
    [SerializeField] AudioSource failedSFX;
    void Start()
    {
        cardPanel.SetActive(false);
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        userInterface = GameObject.Find("UI");
        timeRemaining = 15;
        canOpenCard = true;
    }
    void Update()
    {
        randomValue = Mathf.Clamp(randomValue, 0f, 100f);
        if (isCardChoosing && canOpenCard)
        {
            //StartCoroutine(OpenCard());
            OpenCard();
            timerIsRunning = true;
            timeRemaining = 15;
        }
        else
        if(isSinnerChoosing || isDevilChoosing)
        {
            if(timerIsRunning)
            {
                if(timeRemaining > 0)
                {
                    timeRemaining -= Time.deltaTime;
                    DisplayTime(timeRemaining);
                } else
                {
                    timerIsRunning = false;
                    UseCard();
                    NextTurn();
                }
            }
        } else
        {
            timeRemaining = 15;
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        countdownText.text = seconds.ToString();
    }
    Card card;
    public int openedCardCount;
    Transform nextSlotPosition;
    void OpenCard()
    {
        if (isSinnerChoosing)
        {
            cardPanel.SetActive(true);
            randomValue = Random.value * 100f;

            int selectedCard = 0;
            switch (openedCardCount)
            {
                case 0: nextSlotPosition = slotPosition[0]; break;
                case 1: nextSlotPosition = slotPosition[1]; break;
                case 2: nextSlotPosition = slotPosition[2]; isCardChoosing = false; randomValue = 0; break;
            }
            if (isCommonCards)
            {
                if (randomValue <= 60)
                {
                    selectedCard = Random.Range(0, sinnerCommonCards.Length);
                    card = sinnerCommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 90 && randomValue > 60)
                {
                    selectedCard = Random.Range(0, sinnerUncommonCards.Length);
                    card = sinnerUncommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 100 && randomValue > 90)
                {
                    selectedCard = Random.Range(0, sinnerRareCards.Length);
                    card = sinnerRareCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
            }
            else if (isAllCards)
            {
                if (randomValue <= 30)
                {
                    selectedCard = Random.Range(0, sinnerCommonCards.Length);
                    card = sinnerCommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;

                }
                else if (randomValue <= 65 && randomValue > 30)
                {
                    selectedCard = Random.Range(0, sinnerUncommonCards.Length);
                    card = sinnerUncommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 80 && randomValue > 65)
                {
                    selectedCard = Random.Range(0, sinnerRareCards.Length);
                    card = sinnerRareCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 95 && randomValue > 90)
                {
                    selectedCard = Random.Range(0, sinnerEpicCards.Length);
                    card = sinnerEpicCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 100 && randomValue > 95)
                {
                    selectedCard = Random.Range(0, sinnerLegendaryCards.Length);
                    card = sinnerLegendaryCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
            }
        }
        else if (isDevilChoosing)
        {
            cardPanel.SetActive(true);
            randomValue = Random.value * 100f;

            int selectedCard = 0;
            Transform nextSlotPosition = slotPosition[0];
            switch (openedCardCount)
            {
                case 0: nextSlotPosition = slotPosition[0]; break;
                case 1: nextSlotPosition = slotPosition[1]; break;
                case 2: nextSlotPosition = slotPosition[2]; isCardChoosing = false; randomValue = 0; break;
            }
            if (isCommonCards)
            {
                if (randomValue <= 60)
                {
                    selectedCard = Random.Range(0, devilCommonCards.Length);
                    card = devilCommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 90 && randomValue > 60)
                {
                    selectedCard = Random.Range(0, devilUncommonCards.Length);
                    card = devilUncommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 100 && randomValue > 90)
                {
                    selectedCard = Random.Range(0, devilRareCards.Length);
                    card = devilRareCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
            }
            else if (isAllCards)
            {
                if (randomValue <= 30)
                {
                    selectedCard = Random.Range(0, devilCommonCards.Length);
                    card = devilCommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 65 && randomValue > 30)
                {
                    selectedCard = Random.Range(0, devilUncommonCards.Length);
                    card = devilUncommonCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 80 && randomValue > 65)
                {
                    selectedCard = Random.Range(0, devilRareCards.Length);
                    card = devilRareCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
                else if (randomValue <= 100 && randomValue > 80)
                {
                    selectedCard = Random.Range(0, devilEpicCards.Length);
                    card = devilEpicCards[selectedCard];
                    card.transform.position = nextSlotPosition.position;
                    card.gameObject.SetActive(true);
                    openedCardCount++;
                }
            }
        }
    }

    Card selectedCard;
    public void OpenOptions()
    {
        EventSystem.current.currentSelectedGameObject.transform.Find("Options").gameObject.SetActive(true);
        selectedCard = EventSystem.current.currentSelectedGameObject.GetComponent<Card>();
    }
    public void UseCard()
    {
        cardPanel.SetActive(false);
        gameManager.gameObject.GetComponent<Movement>().canMove = true;
        timeRemaining = 15;
        cardUseSFX.Play();
        openedCardCount = 0;
        if (isSinnerChoosing)
        {
            isSinnerChoosing = false;
            for (int i = 0; i < sinnerCommonCards.Length; i++) { sinnerCommonCards[i].gameObject.SetActive(false); sinnerCommonCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < sinnerUncommonCards.Length; i++) { sinnerUncommonCards[i].gameObject.SetActive(false); sinnerUncommonCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < sinnerRareCards.Length; i++) { sinnerRareCards[i].gameObject.SetActive(false); sinnerRareCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < sinnerEpicCards.Length; i++) { sinnerEpicCards[i].gameObject.SetActive(false); sinnerEpicCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < sinnerLegendaryCards.Length; i++) { sinnerLegendaryCards[i].gameObject.SetActive(false); sinnerLegendaryCards[i].transform.Find("Options").gameObject.SetActive(false); }
        }
        else if (isDevilChoosing)
        {
            isDevilChoosing = false;
            for (int i = 0; i < devilCommonCards.Length; i++) { devilCommonCards[i].gameObject.SetActive(false); devilCommonCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < devilUncommonCards.Length; i++) { devilUncommonCards[i].gameObject.SetActive(false); devilUncommonCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < devilRareCards.Length; i++) { devilRareCards[i].gameObject.SetActive(false); devilRareCards[i].transform.Find("Options").gameObject.SetActive(false); }
            for (int i = 0; i < devilEpicCards.Length; i++) { devilEpicCards[i].gameObject.SetActive(false); devilEpicCards[i].transform.Find("Options").gameObject.SetActive(false); }
        }
    }
    public void AddInventory()
    {
        //Inventory system
        Transform addedCard;
        switch(gameManager.player.name)
        {
            case "Sinner1":
                if(userInterface.transform.Find("Sinner1").GetComponent<InventorySystem>().isInventoryFull)
                {
                    failedSFX.Play();
                } else
                {
                    openedCardCount = 0;
                    addedCard = Instantiate(selectedCard.prefab.transform, userInterface.transform.Find("Sinner1").GetComponent<InventorySystem>().nextCardPosition.parent);
                    addedCard.GetComponent<RectTransform>().sizeDelta = new Vector2(68.83f, 141.11f);
                    addedCard.position = userInterface.transform.Find("Sinner1").GetComponent<InventorySystem>().nextCardPosition.position;
                    addedCard.SetParent(userInterface.transform.Find("Sinner1").GetComponent<InventorySystem>().nextCardPosition.transform);
                    userInterface.transform.Find("Sinner1").GetComponent<InventorySystem>().cardCount++;
                    addedCard.GetComponent<InventoryCard>().cardTitle = selectedCard.gameObject.name;
                    cardPanel.SetActive(false);
                    Invoke("NextTurn", 1.5f);
                    gameManager.gameObject.GetComponent<Movement>().canMove = true;
                    timeRemaining = 15;
                    if (isSinnerChoosing)
                    {
                        isSinnerChoosing = false;
                        for (int i = 0; i < sinnerCommonCards.Length; i++) { sinnerCommonCards[i].gameObject.SetActive(false); sinnerCommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerUncommonCards.Length; i++) { sinnerUncommonCards[i].gameObject.SetActive(false); sinnerUncommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerRareCards.Length; i++) { sinnerRareCards[i].gameObject.SetActive(false); sinnerRareCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerEpicCards.Length; i++) { sinnerEpicCards[i].gameObject.SetActive(false); sinnerEpicCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerLegendaryCards.Length; i++) { sinnerLegendaryCards[i].gameObject.SetActive(false); sinnerLegendaryCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                    }
                }
                break;
            case "Sinner2":
                if (userInterface.transform.Find("Sinner2").GetComponent<InventorySystem>().isInventoryFull)
                {
                    failedSFX.Play();
                }
                else
                {
                    openedCardCount = 0;
                    addedCard = Instantiate(selectedCard.prefab.transform, userInterface.transform.Find("Sinner2").GetComponent<InventorySystem>().nextCardPosition.parent);
                    addedCard.GetComponent<RectTransform>().sizeDelta = new Vector2(68.83f, 141.11f);
                    addedCard.position = userInterface.transform.Find("Sinner2").GetComponent<InventorySystem>().nextCardPosition.position;
                    addedCard.SetParent(userInterface.transform.Find("Sinner2").GetComponent<InventorySystem>().nextCardPosition.transform);
                    userInterface.transform.Find("Sinner2").GetComponent<InventorySystem>().cardCount++;
                    addedCard.GetComponent<InventoryCard>().cardTitle = selectedCard.gameObject.name;
                    cardPanel.SetActive(false);
                    Invoke("NextTurn", 1.5f);
                    gameManager.gameObject.GetComponent<Movement>().canMove = true;
                    timeRemaining = 15;
                    if (isSinnerChoosing)
                    {
                        isSinnerChoosing = false;
                        for (int i = 0; i < sinnerCommonCards.Length; i++) { sinnerCommonCards[i].gameObject.SetActive(false); sinnerCommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerUncommonCards.Length; i++) { sinnerUncommonCards[i].gameObject.SetActive(false); sinnerUncommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerRareCards.Length; i++) { sinnerRareCards[i].gameObject.SetActive(false); sinnerRareCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerEpicCards.Length; i++) { sinnerEpicCards[i].gameObject.SetActive(false); sinnerEpicCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerLegendaryCards.Length; i++) { sinnerLegendaryCards[i].gameObject.SetActive(false); sinnerLegendaryCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                    }
                }
                break;
            case "Sinner3":
                if (userInterface.transform.Find("Sinner3").GetComponent<InventorySystem>().isInventoryFull)
                {
                    failedSFX.Play();
                }
                else
                {
                    openedCardCount = 0;
                    addedCard = Instantiate(selectedCard.prefab.transform, userInterface.transform.Find("Sinner3").GetComponent<InventorySystem>().nextCardPosition.parent);
                    addedCard.GetComponent<RectTransform>().sizeDelta = new Vector2(68.83f, 141.11f);
                    addedCard.position = userInterface.transform.Find("Sinner3").GetComponent<InventorySystem>().nextCardPosition.position;
                    addedCard.SetParent(userInterface.transform.Find("Sinner3").GetComponent<InventorySystem>().nextCardPosition.transform);
                    userInterface.transform.Find("Sinner3").GetComponent<InventorySystem>().cardCount++;
                    addedCard.GetComponent<InventoryCard>().cardTitle = selectedCard.gameObject.name;
                    cardPanel.SetActive(false);
                    Invoke("NextTurn", 1.5f);
                    gameManager.gameObject.GetComponent<Movement>().canMove = true;
                    timeRemaining = 15;
                    if (isSinnerChoosing)
                    {
                        isSinnerChoosing = false;
                        for (int i = 0; i < sinnerCommonCards.Length; i++) { sinnerCommonCards[i].gameObject.SetActive(false); sinnerCommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerUncommonCards.Length; i++) { sinnerUncommonCards[i].gameObject.SetActive(false); sinnerUncommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerRareCards.Length; i++) { sinnerRareCards[i].gameObject.SetActive(false); sinnerRareCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerEpicCards.Length; i++) { sinnerEpicCards[i].gameObject.SetActive(false); sinnerEpicCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < sinnerLegendaryCards.Length; i++) { sinnerLegendaryCards[i].gameObject.SetActive(false); sinnerLegendaryCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                    }
                }
                break;
            case "Devil":
                if (userInterface.transform.Find("Devil").GetComponent<InventorySystem>().isInventoryFull)
                {
                    failedSFX.Play();
                }
                else
                {
                    openedCardCount = 0;
                    addedCard = Instantiate(selectedCard.prefab.transform, userInterface.transform.Find("Devil").GetComponent<InventorySystem>().nextCardPosition.parent);
                    addedCard.GetComponent<RectTransform>().sizeDelta = new Vector2(68.83f, 141.11f);
                    addedCard.position = userInterface.transform.Find("Devil").GetComponent<InventorySystem>().nextCardPosition.position;
                    addedCard.SetParent(userInterface.transform.Find("Devil").GetComponent<InventorySystem>().nextCardPosition.transform);
                    userInterface.transform.Find("Devil").GetComponent<InventorySystem>().cardCount++;
                    addedCard.GetComponent<InventoryCard>().cardTitle = selectedCard.gameObject.name;
                    cardPanel.SetActive(false);
                    Invoke("NextTurn", 1.5f);
                    gameManager.gameObject.GetComponent<Movement>().canMove = true;
                    timeRemaining = 15;
                    if (isDevilChoosing)
                    {
                        isDevilChoosing = false;
                        for (int i = 0; i < devilCommonCards.Length; i++) { devilCommonCards[i].gameObject.SetActive(false); devilCommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < devilUncommonCards.Length; i++) { devilUncommonCards[i].gameObject.SetActive(false); devilUncommonCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < devilRareCards.Length; i++) { devilRareCards[i].gameObject.SetActive(false); devilRareCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                        for (int i = 0; i < devilEpicCards.Length; i++) { devilEpicCards[i].gameObject.SetActive(false); devilEpicCards[i].gameObject.transform.Find("Options").gameObject.SetActive(false); }
                    }
                }
                break;
        }
    }
    void NextTurn()
    {
        gameManager.nextTurn = true;
    }
}
