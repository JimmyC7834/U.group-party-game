using UnityEngine;
using UnityEngine.Pool;

namespace Game.Turret
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class TurretBulletBase : MonoBehaviour
    {
        private ObjectPool<TurretBulletBase> _pool;

        [Header("Bullet Data")] public float _damage = 0f;
        [SerializeField] protected float _speed = 5f;
        [SerializeField] protected float _maxLifespan = 5f;
        protected float _lifespan;


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

        protected abstract void UpdatePosition();

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

        public virtual void Initialize(Vector3 _position, Quaternion _rotation, Transform _target)
        {
        }

        public void SetPool(ObjectPool<TurretBulletBase> pool) => _pool = pool;

        protected void ReturnToPool() => _pool.Release(this);
    }
}