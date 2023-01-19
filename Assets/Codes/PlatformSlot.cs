using UnityEngine;

public class PlatformSlot : MonoBehaviour
{
    public bool isFull;
    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Player")
        {
            isFull = true;
        }
    }
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            isFull = false;
        }
    }
}
