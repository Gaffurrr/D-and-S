using UnityEngine;
using TMPro;

public class InventoryCard : MonoBehaviour
{
    public string cardTitle;
    [SerializeField] private string cardDescription;

    TMP_Text titleText, descriptionText;
    GameObject infoPanel;

    CardSpecifications cardSpesifications;
    GameManager gameManager;

    AudioSource cardUseSFX, failUseSFX;

    void Start()
    {
        gameManager = GameObject.Find("GAME MANAGER").GetComponent<GameManager>();
        cardSpesifications = GameObject.Find("CARD MANAGER").GetComponent<CardSpecifications>();
        infoPanel = transform.Find("InfoPanel").gameObject;
        titleText = transform.Find("InfoPanel").transform.Find("Title").GetComponent<TMP_Text>();
        descriptionText = transform.Find("InfoPanel").transform.Find("Description").GetComponent<TMP_Text>();
        cardUseSFX = GameObject.Find("MUSICS_&_SFX").transform.Find("CardUseSFX").GetComponent<AudioSource>();
        failUseSFX = GameObject.Find("MUSICS_&_SFX").transform.Find("FailUseSFX").GetComponent<AudioSource>();
        infoPanel.SetActive(false);
    }
    void Update()
    {
        titleText.text = cardTitle;
        descriptionText.text = cardDescription;

        CardDescriptions();
    }
    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }
    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }

    public void UseCard()
    {
        if(cardSpesifications.cardDestroyModeOpen)
        {
            Destroy(gameObject);
            GameObject.Find("UI").transform.Find(gameManager.player.name).GetComponent<InventorySystem>().cardCount--;
            cardSpesifications.cardDestroyModeOpen = false;
            gameManager.nextTurn = true;
        } else
        {
            switch(cardTitle)
            {
                case "Deli": cardSpesifications.Deli(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Dost Yard?m?": cardSpesifications.DostYardimi(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Gezgin": cardSpesifications.Gezgin(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Pas": cardSpesifications.Pas(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Ö?ütücü":
                    if(GameObject.Find("UI").transform.Find(gameManager.player.name).GetComponent<InventorySystem>().cardCount > 1)
                    {
                        cardSpesifications.Ogutucu(); cardUseSFX.Play(); Destroy(gameObject);
                    } else failUseSFX.Play();
                    break;
                case "Engelleme": cardSpesifications.Engelleme(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Çelme": cardSpesifications.Celme(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Durdurulamaz": cardSpesifications.Durdurulamaz(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Kalkan": cardSpesifications.Kalkan(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Portal Gezgini": if (gameManager.usableTeleportPointsCount > 0)
                    {
                        cardSpesifications.PortalGezgini(); cardUseSFX.Play(); Destroy(gameObject);
                    } else failUseSFX.Play();
                    break;
                case "Rakip Ö?ütücü": cardSpesifications.RakipOgutucu(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Zincir Tuza??": cardSpesifications.ZincirTuzagi(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "?eytan?n Köpe?i": cardSpesifications.SeytaninKopegi(); cardUseSFX.Play(); Destroy(gameObject); break;
                case "Canland?r": cardSpesifications.Canlandir(); cardUseSFX.Play(); Destroy(gameObject); break;
            }
        }
    }

    void CardDescriptions()
    {
        switch (cardTitle)
        {
            case "Deli": cardDescription = "Kart kullan?c?s?, 1 ad?m geri gider."; break;
            case "Dost Yard?m?": cardDescription = "Bir günahkar?, 1 ad?m ileri do?ru ittirir."; break;
            case "Gezgin": cardDescription = "Kart kullan?c?s?, nerede olursa olsun 2 ad?m mesafeye kadar her yöne ilerleyebilir."; break;
            case "Pas": cardDescription = "Kart kullan?c?s?n?n 1 tur oynamamas?n? sa?lar."; break;
            case "Ö?ütücü": cardDescription = "Envanterden seçilen bir kart? yok eder."; break;
            case "Engelleme": cardDescription = "Seçilen bir oyuncunun 1 tur boyunca oynamas?n? engeller."; break;
            case "Çelme": cardDescription = "Seçilen bir günahkar? 1 ad?m geri götürür. 2 tur boyunca kullan?lmazsa, kart sahibi 1 ad?m geri gider."; break;
            case "Durdurulamaz": cardDescription = "Günahkarlar?n uygulamaya çal??aca?? hiçbir engelleme etkisinden 1 tur için etkilenilmez."; break;
            case "Kalkan": cardDescription = "Kart aktif edildikten sonraki 2 tur boyunca ?eytan'?n sald?r?lar?na kar?? dokunulmazl?k sa?lar."; break;
            case "Portal Gezgini": cardDescription = "Yaln?zca ?eytan'?n seyahat edebildi?i ???nlanma noktalar?na ???nlanabilmeyi sa?lar."; break;
            case "Rakip Ö?ütücü": cardDescription = "Ba?ka bir oyuncunun envanterinde seçilen bir kart? yok eder."; break;
            case "Zincir Tuza??": cardDescription = "Seçilen bir platform üzerine tuzak kurulur. Tuza?a yakalanan oyuncu, 1 tur boyunca oynayamaz. Tuza?a ?eytan da yakalanabilir."; break;
            case "?eytan?n Köpe?i": cardDescription = "Kart al?nd?ktan sonra ölen ilk ki?i, ?eytan'?n taraf?na geçer. Bu özellik, kart ?eytan'?n envanterinde bulundu?u sürece aktiftir."; break;
            case "Canland?r": cardDescription = "Elenmi? bir günahkar? kart kullan?c?s?n?n konumunda diriltir ve oyuna devam edebilmesini saðlar."; break;
        }
    }
}
