using UnityEngine;
using UnityEngine.Pool;

namespace Game.Turret
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TurretBulletBase : MonoBehaviour
    {
        [SerializeField] private Vector3 _dir;
        private ObjectPool<TurretBulletBase> _pool;

        [Header("Bullet Data")] [SerializeField]
        public float _damage = 0f;

        [SerializeField] public float _speed = 5f;
        [SerializeField] protected float _maxLifespan = 5f;
        protected float _lifespan;

        private void Awake()
        {
        }

        private void FixedUpdate()
        {
            _lifespan -= Time.deltaTime;
            if (_lifespan < 0f)
            {
                ReturnToPool();
                return;
            }

            UpdatePosition();
        }

        protected virtual void UpdatePosition()
        {
            transform.Translate(_speed * Time.deltaTime * Vector3.up, Space.Self);
        }

        protected abstract void Hit(Collider2D collider);

        private void OnTriggerEnter2D(Collider2D collider)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                Hit(collider);
        }

        public void Initialize(Vector3 _position)
        {
            transform.position = _position;
            _lifespan = _maxLifespan;
        }

        public void Initialize(Vector3 _position, Quaternion _rotation)
        {
            Initialize(_position);
            transform.rotation = _rotation;
        }

        public void SetPool(ObjectPool<TurretBulletBase> pool) => _pool = pool;

        protected void ReturnToPool() => _pool.Release(this);
    }
}