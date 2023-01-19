using UnityEngine;

public class Card : MonoBehaviour
{
    public string cardTitle;
    public string zoneName;
    public float rarity;
    public bool isSinnerCard;
    public bool isDevilCard;
    public GameObject prefab;

    public void CloseOptions()
    {
        transform.Find("Options").gameObject.SetActive(false);
    }
}
