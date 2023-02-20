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
        private float nextDistance = 0f;
        [SerializeField] private float startTime;
        [SerializeField] private EnemyRouteNode nextPoint;
        [SerializeField] private EnemyStats _stats;
        private ThrowableObject _throwableObject;
        private bool isPicked = false;

        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private SpriteRenderer _bodySprite;

        private void Awake() {
            _throwableObject = GetComponent<ThrowableObject>();
            _throwableObject.OnPickedUp += Picked;
            _throwableObject.OnGrounded += Thrown;

            _stats = GetComponent<EnemyStats>();
            _stats.Killed += Kill;
        }

        private void OnEnable()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
            startPoint = transform.position;
        }

        private void FixedUpdate()
        {
            if (nextPoint == null)
                return;
            Move();
        }

        private void Picked()
        {
            isPicked = true;
        }

        private void Thrown()
        {
            isPicked = false;
            RedirectTo(nextPoint);

            EnemyRouteNode judgeNode = nextPoint.GetNextNode();
            if (judgeNode != null)
            {
                float dist = Vector2.Distance(transform.position, judgeNode.transform.position);
                if (dist <= nextDistance)
                {
                    RedirectTo(judgeNode);
                }
                else
                {
                    float tempDist = Vector2.Distance(transform.position, nextPoint.transform.position);
                    if (dist < tempDist)
                    {
                        RedirectTo(judgeNode);
                    }
                }
            }

        }

        private void Move()
        {
            if (isPicked) return;
            // bad calculation, fail when gen in node 0
            // redirect to next point when is thrown over current point
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
            _stats.Initialize(data);
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

            EnemyRouteNode tempNode = nextPoint.GetNextNode();
            if(tempNode != null)
            {
                nextDistance = Vector2.Distance(nextPoint.transform.position, tempNode.transform.position);
            }

            startTime = Time.time;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                PlayerStats playerStats = collision.collider.GetComponent<PlayerStats>();
                playerStats?.Hit(_stats.strength);
            }
        }
    }
}