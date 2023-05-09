using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private List<Point> _points = new();
    [SerializeField] private DrawLine _linePrefab;

    private void Start()
    {
        Instantiate();
    }

    public DrawLine Instantiate()
    {
        Character character = GetComponent<Character>();
        DrawLine line = GameObject.Instantiate(_linePrefab);
        Point point = GetComponent<Point>();

        line.SetLine(_color, point, _points);
        character.SetLine(line);
        Level.singleton.AddLine(line);

        return line;
    }
}
