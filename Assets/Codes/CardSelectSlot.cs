using UnityEngine;

public class CardSelectSlot : MonoBehaviour
{
    public bool isFull;
    void Update()
    {
        if (transform.childCount > 2) isFull = true;
        else isFull = false;
    }
}
