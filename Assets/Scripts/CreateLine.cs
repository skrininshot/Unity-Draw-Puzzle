using System.Collections.Generic;
using UnityEngine;

public class CreateLine : MonoBehaviour
{
    [SerializeField] private Color _color;
    [SerializeField] private List<Point> _points = new();

    private void Start()
    {
        Instantiate();
    }

    public DrawLine Instantiate()
    {
        Character character = GetComponent<Character>();
        Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Line/LineDrawer.prefab", typeof(DrawLine));
        DrawLine line = GameObject.Instantiate(prefab) as DrawLine;
        Point point = GetComponent<Point>();

        line.SetLine(_color, point, _points);
        character.SetLine(line);
        Level.singleton.AddLine(line);

        return line;
    }
}
