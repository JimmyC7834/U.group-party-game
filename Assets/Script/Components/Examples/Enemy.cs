using Game.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Enemys
{

    [RequireComponent(typeof(Throwable))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(TargetLoader))]
    public class Enemy : MonoBehaviour
    {
        private ObjectPool<Enemy> _pool;

        [SerializeField] protected EnemySO _enemySO;
        [SerializeField] private SpriteRenderer _bodySprite;

        private Health _health;
        [SerializeField] private string _targetLayer = "Player";

        private TargetLoader _targetLoader;
        private Transform _target;

        public void SetPool(ObjectPool<Enemy> pool) => _pool = pool;
        protected void ReturnToPool() => _pool.Release(this);

        private void Awake()
        {
            _health = GetComponent<Health>();
            // _health.InitHealth(_enemySO.maxHealth);
            _health.OnDead += ReturnToPool;

            // TODO: Think about the usage of target loader
            _targetLoader = GetComponent<TargetLoader>();
            _targetLoader.SetLayer(_targetLayer);
            _targetLoader.SetRange(100);
        }

        private void FixedUpdate()
        {
            AimTarget();
            Move();
        }

        public void Initialize(EnemySO enemySO)
        {
            _enemySO = enemySO;
            _bodySprite.sprite = _enemySO.sprite;
            _health.InitHealth(_enemySO.maxHealth);
        }


        private void Move()
        {

        }
        private void AimTarget()
        {

        }
        public void RedirectTo(EnemyRouteNode node)
        {

        }

        // Attack
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
            {
                Debug.Log(collision);
                Health health = collision.gameObject.GetComponent<Health>();
                Debug.Log(health);
                Attack(health);
            }
        }

        private void Attack(Health target)
        {
            target.Damage(_enemySO.strength);
            _health.Suicide();
        }
    }

}
