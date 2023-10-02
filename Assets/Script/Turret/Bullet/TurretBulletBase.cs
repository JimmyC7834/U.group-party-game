using UnityEngine;
using UnityEngine.Pool;

namespace Game.Turret
{
    [RequireComponent(typeof(Collider2D))]
    [RequireComponent(typeof(TargetLoader))]
    [RequireComponent(typeof(Timer))]
    public abstract class TurretBulletBase : MonoBehaviour
    {
        private ObjectPool<TurretBulletBase> _pool;
        protected TargetLoader _targetLoader;
        protected Timer _timer;

        [Header("Bullet Data")]
        [SerializeField] public float _damage = 0f;
        [SerializeField] protected float _speed = 5f;
        [SerializeField] protected float _maxLifespan = 5f; // unit: ms

        protected virtual void Awake()
        {
            _timer = GetComponent<Timer>();
            _timer.SetSec(_maxLifespan);
            _timer.SetCallBack(ReturnToPool);
            _timer.Time();
        }
        private void FixedUpdate()
        {
            UpdatePosition();
        }

        public void SetPool(ObjectPool<TurretBulletBase> pool) => _pool = pool;
        protected void ReturnToPool() => _pool.Release(this);

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
        }
        public void Initialize(Vector3 _position, Quaternion _rotation)
        {
            Initialize(_position);
            transform.rotation = _rotation;
        }
        public virtual void Initialize(Vector3 _position, Quaternion _rotation, Transform _target) { }
    }
}