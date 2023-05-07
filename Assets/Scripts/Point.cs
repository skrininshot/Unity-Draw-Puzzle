using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class Point : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public bool isTrigger { get; private set; }
    [field: SerializeField] public PointType type { get; private set; }

    private void Awake()
    {
       GetComponent<Rigidbody2D>().isKinematic = true;
       GetComponent<CircleCollider2D>().isTrigger = true;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (type.Equals(PointType.End))
            isTrigger = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (type.Equals(PointType.Start))
            isTrigger = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isTrigger = false;
    }

    
    public bool Equals(Point point)
    {
        return GetInstanceID() == point.gameObject.GetInstanceID();
    }
}

public enum PointType
{
    Start,
    End
}