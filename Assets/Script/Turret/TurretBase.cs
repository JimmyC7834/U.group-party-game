using System.Collections.Generic;
using UnityEngine;

using Game.Core;

namespace Game.Turret
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected GameplayService _gameplayService;
        [SerializeField] protected Transform _shootingPoint;
        [SerializeField] protected CircleCollider2D _shootingTrigger;
        [SerializeField] protected Transform _target;
        [SerializeField] protected Transform _partToRotate;
        protected BulletManager _bulletManager;
        protected PowerStation _powerStation;

        [Space] [Header("Turret Data")] [SerializeField]
        protected float _shootingRange = 3f;

        [SerializeField] protected float _cooldown = 0.5f;
        [SerializeField] protected float _rotateSpeed = 5f;
        [SerializeField] protected float _durability = 100f;
        [SerializeField] protected float _consumptionPerBullet = 5f;
        [SerializeField] protected GameObject _bulletPrefab;
        [SerializeField] protected List<Collider2D> _targetQueue;
        [SerializeField] protected bool _isEnergySuppplied = false;
        private bool _enable = true;

        protected bool ShouldFire => IsEnable() && _target != null;

        private void Awake()
        {
            _bulletManager = gameObject.AddComponent<BulletManager>();
            _bulletManager.Initialize(transform, _bulletPrefab);

            _shootingTrigger = gameObject.AddComponent<CircleCollider2D>();
            _shootingTrigger.radius = _shootingRange;
            _shootingTrigger.isTrigger = true;

            _targetQueue = new List<Collider2D>();
        }

        private void Start()
        {
            GetComponent<ThrowableObject>().OnGrounded += Enable;
            GetComponent<ThrowableObject>().OnLaunch += Disable;
        }

        private void LateUpdate() {
            StopSupplied();
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

        // not safe, to be fixed
        public virtual void EnergySupplied()
        {
            _isEnergySuppplied = true;
        }

        public virtual void StopSupplied()
        {
            _isEnergySuppplied = false;
        }

        public virtual bool IsSupplied()
        {
            return _isEnergySuppplied;
        }

        protected abstract void AimTarget();

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