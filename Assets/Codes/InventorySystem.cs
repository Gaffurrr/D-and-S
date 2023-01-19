using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [Header("Inventory System")]
    public int cardCount;
    [SerializeField] Transform[] inventorySlots;
    public Transform nextCardPosition;
    public bool isInventoryFull;
    void Start()
    {
        cardCount = 0;
    }
    void Update()
    {
        if(!inventorySlots[0].GetComponent<InventorySlot>().isFull)
        {
            nextCardPosition = inventorySlots[0];
            isInventoryFull = false;
        } else if(!inventorySlots[1].GetComponent<InventorySlot>().isFull)
        {
            nextCardPosition = inventorySlots[1];
            isInventoryFull = false;
        } else if (!inventorySlots[2].GetComponent<InventorySlot>().isFull)
        {
            nextCardPosition = inventorySlots[2];
            isInventoryFull = false;
        }
        else
        {
            isInventoryFull = true;
        }
    }
}
