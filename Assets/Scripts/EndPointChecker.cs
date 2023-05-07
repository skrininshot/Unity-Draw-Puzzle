using System.Collections.Generic;
using UnityEngine;

public delegate void GetPoint(Point point);

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
public class EndPointChecker : MonoBehaviour
{
    public event GetPoint onGetPoint = delegate { };
    private List<Point> _points;

    private void Awake()
    {
        GetComponent<Rigidbody2D>().isKinematic = true;
        GetComponent<CircleCollider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Point>(out Point point))
            CheckPoint(point);
    }

    public void Set(List<Point> points)
    {
        _points = points;
    }

    private void CheckPoint(Point point)
    {
        if (point is null) return;

        if (point.type is PointType.Start) return;

        if (_points.Contains(point))
            onGetPoint.Invoke(point);
    }
}
