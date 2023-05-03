using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawLine : MonoBehaviour
{
    private LineRenderer _lineRenderer;
    private Camera _camera;
    [SerializeField, Min(0.01f)] private float _mousePositionUpdateFreq = 0.1f;
    [SerializeField] private GameObject _circlePointPrefab;
    private Transform _circlePoint;
    private GameObject _circleStartPoint;
    private bool _isDrawing;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _camera = Camera.main;
        _circlePoint = Instantiate(_circlePointPrefab, Vector3.zero, Quaternion.identity).transform;
        _circleStartPoint = Instantiate(_circlePointPrefab, Vector3.zero, Quaternion.identity);
        StartCoroutine(UpdateMousePosition());
    }

    private IEnumerator UpdateMousePosition()
    {
        WaitForSeconds updateTime = new(_mousePositionUpdateFreq);

        while (true)
        {
            _isDrawing = Input.GetMouseButton(0);

            if (_isDrawing)
            {
                Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                _circlePoint.position = mousePosition;
                AddPoint(mousePosition);
            }
            else
            {
                CheckLine();
            }
            
            yield return updateTime;
        }
    }

    private void AddPoint(Vector3 pointPosition)
    {   
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, pointPosition);

        if (!_circleStartPoint.activeSelf)
        {
            _circleStartPoint.SetActive(true);
            _circleStartPoint.transform.position = pointPosition;
        }
    }

    private void CheckLine()
    {
        //need to edit
        CancelLine();
    }

    private void CancelLine()
    {
        _lineRenderer.positionCount = 0;
        _circleStartPoint.SetActive(false);
    }
}
