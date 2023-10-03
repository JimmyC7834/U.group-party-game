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
        [SerializeField] protected float _maxLifespan = 5f;
        [SerializeField] protected string _targetLayer = "Enemy";

        private bool _hasHit = false;

        protected virtual void Awake()
        {
            _timer = GetComponent<Timer>();
            _timer.SetSec(_maxLifespan);
            _timer.SetCallBack(ReturnToPool);
        }

        private void OnEnable()
        {
            _hasHit = false;
            _timer.Time();
        }

        private void FixedUpdate()
        {
            UpdatePosition();
        }

        public void SetPool(ObjectPool<TurretBulletBase> pool) => _pool = pool;
        protected void ReturnToPool()
        {
            // bullet are too fast, release and respawn at the same time
            try
            {
                _pool.Release(this);
            }
            catch (System.InvalidOperationException e)
            {
                Debug.Log(e);
            }

        }

        protected abstract void UpdatePosition();
        protected abstract void Hit(Collider2D collider);


        private void OnTriggerEnter2D(Collider2D collider)
        {
            // Bug: Hit player multiple times by one bullet
            if (collider.gameObject.layer == LayerMask.NameToLayer(_targetLayer))
            {
                Debug.Log(collider);
                if (!_hasHit) Hit(collider);
                _hasHit = true;

            }
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

        public void SetTargetLayer(string layer) => _targetLayer = layer;
    }
}