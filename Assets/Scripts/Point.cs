using UnityEngine;
using UnityEngine.EventSystems;


public class Point : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [HideInInspector] public bool isTrigger { get; private set; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isTrigger = true;
        Debug.Log($"Entered {gameObject.name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isTrigger = false;
        Debug.Log($"Exit {gameObject.name}");
    }
}