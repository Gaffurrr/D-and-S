using UnityEngine;
using UnityEngine.UI;

public class DevilSpesifications : MonoBehaviour
{
    public float ultiValue;
    [SerializeField] Slider ultiBar;

    public bool isUltiUsable;
    public bool isUltiUsed;

    [SerializeField] GameObject ultiReadyText;
    void Start()
    {
        ultiValue = 0;
        isUltiUsable = false;
    }
    void Update()
    {
        ultiValue = Mathf.Clamp(ultiValue, 0f, 100f);

        ultiBar.value = ultiValue;

        if(ultiValue >= 100)
        {
            isUltiUsable = true;
            ultiReadyText.SetActive(true);
        } else
        {
            isUltiUsable = false;
            ultiReadyText.SetActive(false);
        }

        if(isUltiUsed)
        {
            if(ultiValue < 0)
            {
                isUltiUsed = false;
                ultiValue = 0;
            }
        }
    }
}
