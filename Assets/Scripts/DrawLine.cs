using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void FinishReached();
public class DrawLine : MonoBehaviour
{
    public event FinishReached onFinishReached = delegate { };
    private Point _startPoint;
    private Point _endPoint;

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

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main;
    }

    public void SetLine(Color color, Point startPoint, Point endPoint)
    {
        _circleDot = CreateCirclePoint(color).transform;
        _circleStartDot = CreateCirclePoint(color);
        _lineRenderer.startColor = color;
        _lineRenderer.endColor = color;

        _startPoint = startPoint;
        _endPoint = endPoint;

        StartCoroutine(UpdateMousePosition());
    }

    private GameObject CreateCirclePoint(Color color)
    {
        GameObject circleDot = Instantiate(_circleDotPrefab, Vector3.zero, Quaternion.identity);
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
            _circleDot.position = mousePosition;
            AddPoint(mousePosition);

            if (_endPoint.isTrigger)
                Finish();
        }
    }

    private void StartDrawing()
    {
        if (!_startPoint.isTrigger) return;

        _isDrawing = true;
        _circleDot.gameObject.SetActive(true);
        _circleStartDot.SetActive(true);

        Vector3 startPosition = _startPoint.transform.position;
        _circleStartDot.transform.position = startPosition;
        AddPoint(startPosition);
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

        if (!_circleStartDot.activeSelf)
        {
            _circleStartDot.SetActive(true);
            _circleStartDot.transform.position = pointPosition;
        }
    }

    private void Finish()
    { 
        StopAllCoroutines();
        
        Vector3 endPosition = _endPoint.transform.position;
        _circleDot.position = endPosition;
        AddPoint(endPosition);
        EndDrawing();
        
        onFinishReached.Invoke();
        Debug.Log("You have reached the finish!");
    }

    private void CancelLine()
    {
        EndDrawing();
        _lineRenderer.positionCount = 0;
        _linePositions.Clear();
        _circleDot.gameObject.SetActive(false);
        _circleStartDot.SetActive(false);
    }
}