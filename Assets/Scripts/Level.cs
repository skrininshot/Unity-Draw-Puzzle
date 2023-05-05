using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public delegate void AllLinesFinished();
public delegate void GameOver();
public class Level : MonoBehaviour
{
    public event AllLinesFinished onAllLinesFinished = delegate{ };
    public event GameOver onGameOver = delegate{ };
    public static Level singleton;
    [SerializeField] private List<LineTypeOption> _lineTypes = new();
    private List<DrawLine> _lines = new();
    private int _finishedLinesCount = 0;
    private bool _isGameOver;

    private void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
    }

    private void Start()
    {
        foreach (LineTypeOption option in _lineTypes)
        {
            DrawLine line = option.Instantiate();
            _lines.Add(line);
            line.onFinishReached += OnLineFinished;
        }
    }

    private void OnEnable()
    {
        if (_lines.Count.Equals(0)) return;

        foreach (var line in _lines)
            line.onFinishReached += OnLineFinished;
    }

    private void OnDisable()
    {
        foreach (var line in _lines)
            line.onFinishReached -= OnLineFinished;
    }

    private void OnLineFinished()
    {
        _finishedLinesCount++;

        if (_finishedLinesCount.Equals(_lines.Count))
            onAllLinesFinished.Invoke();
    }

    public void GameOver()
    {
        if (!_isGameOver)
        {
            _isGameOver = true;
            onGameOver.Invoke();
        }       
    }
}

[System.Serializable]
public class LineTypeOption
{
    [SerializeField] private Color _color;
    [SerializeField] private Character _character;
    [SerializeField] private Point _endPoint;

    public DrawLine Instantiate()
    {
        Object prefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/LineDrawer.prefab", typeof(DrawLine));
        DrawLine line = GameObject.Instantiate(prefab) as DrawLine;
        line.SetLine(_color, _character.point, _endPoint);
        _character.SetLine(line);

        return line;
    }
}