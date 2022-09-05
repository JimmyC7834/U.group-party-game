using UnityEngine;

using Game.Data;

namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private float distance;
        [SerializeField] private float startTime;
        public Transform nextPoint;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _bodySprite = default;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            startPoint = transform.position;
        }

        private void FixedUpdate()
        {
            Move();
        }

        private void Move()
        {
            _rigidbody.position = Vector2.Lerp(startPoint, nextPoint.position, (Time.time - startTime) * speed * speedMultiplier / distance); 
            // _rigidbody.MovePosition(Vector2.MoveTowards(_rigidbody.position, (Vector2) nextPoint.position, speed * speedMultiplier * Time.fixedDeltaTime));
        }

        public void Initialize(EnemySO data)
        {
            _bodySprite.sprite = data.sprite;
            speed = data.speed;
        }

        public void RedirectTo(Transform targetPoint)
        {
            startPoint = nextPoint.position;
            nextPoint = targetPoint;
            distance = Vector2.Distance(startPoint, nextPoint.position);
            startTime = Time.time;
        }
    }
}