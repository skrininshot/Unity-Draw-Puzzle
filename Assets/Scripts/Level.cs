using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public delegate void AllLinesFinished();
public delegate void GameOver(GameOverReason reason);
public delegate void Victory();
public class Level : MonoBehaviour
{
    public event AllLinesFinished onAllLinesFinished = delegate{ };
    public event GameOver onGameOver = delegate{ };
    public event Victory onVictory = delegate{ };
    public static Level singleton;
    private List<DrawLine> _lines = new();
    private int _finishedLinesCount = 0;
    private bool _isGameOver;
    private bool _isVictory;

    private void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
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

    public void GameOver(GameOverReason reason)
    {
        if (!_isGameOver)
        {
            _isGameOver = true;
            onGameOver.Invoke(reason);
        }       
    }

    public void AddLine(DrawLine line)
    {
        _lines.Add(line);
        line.onFinishReached += OnLineFinished;
    }

    public void Victory()
    {
        if (!_isVictory)
        {
            _isVictory = true;
            onVictory.Invoke();
        }       
    }
}

public enum GameOverReason
{
    HitAnother,
    HitObstacle
}