using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class CharacterCollider : MonoBehaviour
{
    private Character _host;
    private Level _level;

    private void Start()
    {
        _host = transform.parent.gameObject.GetComponent<Character>();
        _level = Level.singleton;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Character>(out Character character))
        {
            if (character == _host) return;

            _level.GameOver(GameOverReason.HitObstacle);
        }
    }
}