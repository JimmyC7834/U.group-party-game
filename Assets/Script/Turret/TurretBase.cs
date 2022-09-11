using System.Collections.Generic;
using UnityEngine;

namespace Game.Turret
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected GameplayService _gameplayService;
        [SerializeField] protected Transform _shootingPoint;
        [SerializeField] protected CircleCollider2D _shootingCollider;
        [SerializeField] protected Transform _target;
        [SerializeField] protected Transform _partToRotate;
        protected BulletManager _bulletManager;

        [Space]
        [Header("Turret Data")]
        [SerializeField] protected float _shootingRange = 3f;
        [SerializeField] protected float _cooldown = 0.5f;
        [SerializeField] protected float _rotateSpeed = 5f;
        [SerializeField] protected float _durability = 100f;
        [SerializeField] protected float _consumptionPerBullet = 5f;
        [SerializeField] protected GameObject _bulletPrefab;
        [SerializeField] protected List<Collider2D> _targetQueue;
        private bool _enable = true;

        private void Awake()
        {
            _partToRotate = transform;
            _bulletManager = gameObject.AddComponent<BulletManager>();
            _bulletManager._prefab = _bulletPrefab;
            _shootingCollider = gameObject.AddComponent<CircleCollider2D>() as CircleCollider2D;
            _shootingCollider.radius = _shootingRange;
            _shootingCollider.isTrigger = true;
            _targetQueue = new List<Collider2D>();
        }

        private void Start()
        {
            GetComponent<ThrowableObject>().OnGrounded += Enable;
            GetComponent<ThrowableObject>().OnLaunch += Disable;
        }

        protected virtual void Disable()
        {
            _enable = false;
        }

        protected virtual void Enable()
        {
            _enable = true;
        }

        protected virtual bool IsEnable()
        {
            return _enable;
        }

        protected abstract void AimTarget();

        protected abstract void UpdateTarget();

        protected abstract void Shoot();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _targetQueue.Add(other);
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            _targetQueue.Remove(other);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _shootingRange);
        }
    }
}