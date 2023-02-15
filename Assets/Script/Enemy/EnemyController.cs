using Game.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Enemy
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class EnemyController : MonoBehaviour
    {
        private ObjectPool<EnemyController> _pool;
        [SerializeField] private float speed;
        [SerializeField] private float speedMultiplier = 1;
        [SerializeField] private Vector2 startPoint;
        [SerializeField] private float distance;
        [SerializeField] private float startTime;
        [SerializeField] private EnemyRouteNode nextPoint;
        [SerializeField] private EnemyStatus _status;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _bodySprite;

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            startPoint = transform.position;
            if (_status != null)
            {
                _status = GetComponent<EnemyStatus>();
            }
            else
            {
                _status = gameObject.AddComponent<EnemyStatus>();
                _status.Killed += Kill;
            }
        }

        private void FixedUpdate()
        {
            if (nextPoint == null)
                return;
            Move();
        }

        private void Move()
        {
            // bad calculation, fail when gen in node 0
            float rate = (Time.time - startTime) * speed * speedMultiplier / distance;
            if (rate >= 1)
            {
                transform.position = nextPoint.transform.position;
                RedirectTo(nextPoint.GetNextNode());
            }

            _rigidbody.position = Vector2.Lerp(startPoint, nextPoint.transform.position, rate);
        }

        public void Initialize(EnemySO data)
        {
            _bodySprite.sprite = data.sprite;
            speed = data.speed;
            _status.Initialize(data);
        }

        public void SetPool(ObjectPool<EnemyController> pool) => _pool = pool;

        private void Kill()
        {
            _pool.Release(this);
        }

        public void RedirectTo(EnemyRouteNode nextNode)
        {
            if (nextNode == null)
            {
                Kill();
                return;
            }

            startPoint = transform.position;
            nextPoint = nextNode;
            distance = Vector2.Distance(startPoint, nextPoint.transform.position) + .01f;
            startTime = Time.time;
        }
    }
}