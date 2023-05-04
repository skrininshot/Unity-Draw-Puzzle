using UnityEngine;
using System.Collections.Generic;

public class Character : MonoBehaviour
{
    [SerializeField] private DrawLine _drawLine;
    [SerializeField] private float _moveDuration = 2f;
    private bool _isMoving;
    private Animator _animator;
    private SpriteRenderer _sprite;
    private Level _level;
    private List<Vector3> _linePositions;
    private float _zAxis;
    private float _totalLength = 0;
    private float _currentDistance = 0;
    private float _totalTime = 0;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _sprite = GetComponent<SpriteRenderer>();
        _level = Level.singleton; 
        _zAxis = transform.position.z;
    }

    private void OnEnable()
    {
        _level.onAllLinesFinished += StartMoving;
    }

    private void OnDisable()
    {
        _level.onAllLinesFinished -= StartMoving;
    }

    private void StartMoving()
    {
        _isMoving = true;
        _animator.SetBool("isMoving", true);
        _linePositions = _drawLine.linePositions;

        for (int i = 1; i < _linePositions.Count; i++)
            _totalLength += Vector3.Distance(_linePositions[i - 1], _linePositions[i]);
    }

    private void StopMoving()
    {
        _isMoving = false;
        _animator.SetBool("isMoving", false);
    }

    private void Update()
    {
        if (!_isMoving) return;

        float currentDistanceRatio = _totalTime / _moveDuration;
        _currentDistance = currentDistanceRatio * _totalLength;

        Vector3 currentTargetPosition = GetTargetPosition(_currentDistance);
        currentTargetPosition.z = _zAxis;

        Vector3 direction = currentTargetPosition - transform.position;
        _sprite.flipX = (direction.x > 0);
        
        transform.position = currentTargetPosition;

        _totalTime += Time.deltaTime;

        if (_currentDistance >= _totalLength)
            StopMoving();
    }

    private Vector3 GetTargetPosition(float distance)
    {
        float currentDistance = 0f;

        for (int i = 1; i < _linePositions.Count; i++)
        {
            float segmentDistance = Vector3.Distance(_linePositions[i - 1], _linePositions[i]);

            if (currentDistance + segmentDistance > distance)
            {
                float remainingDistance = distance - currentDistance;
                float segmentRatio = remainingDistance / segmentDistance;
                
                return Vector3.Lerp(_linePositions[i - 1], _linePositions[i], segmentRatio);
            }

            currentDistance += segmentDistance;
        }

        return _linePositions[_linePositions.Count - 1];
    }
}