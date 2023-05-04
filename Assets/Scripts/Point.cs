using UnityEngine;
using UnityEngine.EventSystems;

public class Point : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public bool isTrigger { get; private set; }
    [SerializeField] private PointType _type;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_type.Equals(PointType.End))
            isTrigger = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_type.Equals(PointType.Start))
            isTrigger = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isTrigger = false;
    }
}

public enum PointType
{
    Start,
    End
}