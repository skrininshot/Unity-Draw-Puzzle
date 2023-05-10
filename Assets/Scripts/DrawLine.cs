using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FinishReached();
public class DrawLine : MonoBehaviour
{
    public event FinishReached onFinishReached = delegate { };
    private Point _startPoint;

    [Header ("Line Properties")]
    
    [SerializeField] private GameObject _circleDotPrefab;
    [SerializeField] private float _distanceBetweenPoints = 1f;

    private LineRenderer _lineRenderer;
    private Camera _camera;
    private Transform _circleDot;
    private GameObject _circleStartDot;
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
    private bool _isDrawing;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main;
    }

    public void SetLine(Color color, Point startPoint, List<Point> points)
    {
        _startPointPosition = startPoint.transform.position;
        _startPointPosition.z = 0;
        _circleStartDot = CreateCirclePoint(color, _startPointPosition);
        _circleDot = CreateCirclePoint(color, Vector3.zero).transform;

        SetColor(color);

        _endPointChecker = _circleDot.gameObject.AddComponent<EndPointChecker>();
        _endPointChecker.Set(points);
        _endPointChecker.onGetPoint += Finish;
        
        _startPoint = startPoint;
    }

    private void SetColor(Color color)
    {
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;
    }

    private GameObject CreateCirclePoint(Color color, Vector3 position)
    {
        GameObject circleDot = Instantiate(_circleDotPrefab, position, Quaternion.identity);
        circleDot.GetComponent<SpriteRenderer>().color = color;
        circleDot.SetActive(false);

        return circleDot;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {   
            StartDrawing();
            
            return;
        }

        if (Input.GetMouseButton(0))
        {
            if (_isDrawing)
            {
                Vector3 mousePosition = GetMousePosition();

                if (Vector3.Distance(mousePosition, 
                _lineRenderer.GetPosition(_lineRenderer.positionCount - 1)) > _distanceBetweenPoints)
                    AddPoint(mousePosition);
            }

            return;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (_isDrawing)
                CancelLine();
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

    private void AddPoint(Vector3 pointPosition)
    {   
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pointPosition);
        _linePositions.Add(_lineRenderer.GetPosition(_lineRenderer.positionCount - 1));
        _circleDot.position = pointPosition;
    }

    private void Finish(Point point)
    { 
        _isDrawing = false;

        Vector3 endPosition = point.transform.position;
        endPosition.z = 0;
        _circleDot.position = endPosition;
        AddPoint(endPosition);

        onFinishReached.Invoke();
        enabled = false;
    }

    private void CancelLine()
    {
        _isDrawing = false;
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