using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    public float popSize = 1.1f; 

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * popSize; 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale; 
    }
}