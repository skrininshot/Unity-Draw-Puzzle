using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private DrawLine _drawLine;
    [SerializeField] private float _walkSpeed;
    private bool _isMoving;
    private Animator _animator;
    private Level _level;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _level = Level.singleton;
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
        _animator.SetBool("isMoving", true);
    }

    private void StopMoving()
    {
        _animator.SetBool("isMoving", false);
    }
}
