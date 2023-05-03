using System.Collections.Generic;
using UnityEngine;

public delegate void AllLinesFinished();
public class Level : MonoBehaviour
{
    public event AllLinesFinished onAllLinesFinishedEvent = delegate{ };
    public static Level singleton;
    [SerializeField] private List<DrawLine> _lines = new();
    private int _finishedLinesCount = 0;

    private void Awake()
    {
        if (singleton != null)
            Destroy(gameObject);
        else
            singleton = this;
    }

    private void OnEnable()
    {
        foreach (var line in _lines)
            line.onFinishReached += OnLineFinished;
    }

    private void OnLineFinished()
    {
        _finishedLinesCount++;

        if (_finishedLinesCount.Equals(_lines.Count))
            onAllLinesFinishedEvent.Invoke();
    }

    private void OnDisable()
    {
        foreach (var line in _lines)
            line.onFinishReached -= OnLineFinished;
    }
}
