using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public bool isFull;
    void Update()
    {
        if (transform.childCount > 0) isFull = true;
        else isFull = false;
    }
}
