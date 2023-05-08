using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FinishReached();
public class DrawLine : MonoBehaviour
{
    public event FinishReached onFinishReached = delegate { };
    private Point _startPoint;

    [Header ("Line Properties")]
    [SerializeField, Min (0.1f)] private float _simplifyToleranceAfterDrawing = 0.25f;
    [SerializeField, Min(0.01f)] private float _mousePositionUpdateFreq = 0.1f;
    [SerializeField] private GameObject _circleDotPrefab;

    private LineRenderer _lineRenderer;
    private Camera _camera;
    private Transform _circleDot;
    private GameObject _circleStartDot;
    private bool _isDrawing;
    public List<Vector3> linePositions
    {
        get
        {
            return new List<Vector3>(_linePositions);
        }
    }
    private List<Vector3> _linePositions = new();
    private EndPointChecker _endPointChecker;
    private Vector3 _startPointPosition;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main;
    }

    public void SetLine(Color color, Point startPoint, List<Point> points)
    {
        _startPointPosition = startPoint.transform.position;
        _circleStartDot = CreateCirclePoint(color, _startPointPosition);
        _circleDot = CreateCirclePoint(color, Vector3.zero).transform;

        SetColor(color);

        _endPointChecker = _circleDot.gameObject.AddComponent<EndPointChecker>();
        _endPointChecker.Set(points);
        _endPointChecker.onGetPoint += Finish;
        
        _startPoint = startPoint;

        StartCoroutine(UpdateMousePosition());
    }

    private void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    private GameObject CreateCirclePoint(Color color, Vector3 position)
    {
        position.z = 0;
        GameObject circleDot = Instantiate(_circleDotPrefab, position, Quaternion.identity);
        circleDot.GetComponent<SpriteRenderer>().color = color;
        circleDot.SetActive(false);

        return circleDot;
    }

    private IEnumerator UpdateMousePosition()
    {
        WaitForSeconds updateTime = new(_mousePositionUpdateFreq);

        while (true)
        {
            Drawing();
            
            yield return updateTime;
        }
    }

    private void Drawing()
    {
        bool isClick = Input.GetMouseButton(0);

        if (isClick)
        {
            if (!_isDrawing)
                StartDrawing();
        } 
        else
        {
            if (_isDrawing)
                CancelLine();
        }
            
        if (_isDrawing)
        {
            Vector3 mousePosition = GetMousePosition();
            AddPoint(mousePosition);
        }
    }

    private void StartDrawing()
    {
        if (!_startPoint.isTrigger) return;

        _isDrawing = true;
        _circleDot.gameObject.SetActive(true);
        _circleStartDot.SetActive(true);
        
        AddPoint(_startPointPosition);
    }

    private Vector3 GetMousePosition()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        return mousePosition;
    }

    private void EndDrawing()
    {
        _isDrawing = false;

        for (int i = 0; i < _lineRenderer.positionCount; i++)
            _linePositions.Add(_lineRenderer.GetPosition(i));

        _lineRenderer.Simplify(_simplifyToleranceAfterDrawing);
    }

    private void AddPoint(Vector3 pointPosition)
    {   
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pointPosition);
        _circleDot.position = pointPosition;
    }

    private void Finish(Point point)
    { 
        StopAllCoroutines();
        
        Vector3 endPosition = point.transform.position;
        endPosition.z = 0;
        _circleDot.position = endPosition;
        AddPoint(endPosition);
        EndDrawing();
        
        onFinishReached.Invoke();
    }

    private void CancelLine()
    {
        EndDrawing();
        _lineRenderer.positionCount = 0;
        _linePositions.Clear();
        _circleDot.gameObject.SetActive(false);
        _circleStartDot.SetActive(false);
    }

    private void OnDisable()
    {
        _endPointChecker.onGetPoint -= Finish;
    }
}