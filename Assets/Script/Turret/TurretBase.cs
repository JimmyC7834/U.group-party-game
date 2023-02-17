using System.Collections.Generic;
using System.Linq;
using Game.Core;
using UnityEngine;

namespace Game.Turret
{
    [RequireComponent(typeof(ThrowableObject))]
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected ThrowableObject _throwableObject;
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
        public bool isEnable { get; private set; }

        protected bool ShouldFire => isEnable && _target != null;
        protected bool CanFire => _isEnergySuppplied && _durability >= _consumptionPerBullet;

        private void Awake()
        {
            _bulletManager = gameObject.AddComponent<BulletManager>();
            _bulletManager.Initialize(transform, _bulletPrefab);

            _shootingTrigger.radius = _shootingRange;
            _shootingTrigger.isTrigger = true;

            _targetQueue = new List<Collider2D>();

            _throwableObject = GetComponent<ThrowableObject>();
            _throwableObject.OnPickedUp += Disable;
            _throwableObject.OnGrounded += Enable;
        }

        protected virtual void Disable()
        {
            isEnable = false;
            CancelInvoke("CheckAndShoot");
        }

        protected virtual void Enable()
        {
            isEnable = true;
            CheckAndShoot();
            InvokeRepeating("CheckAndShoot", 1f, _cooldown);
        }

        public virtual void EnergySupplied()
        {
            _isEnergySuppplied = true;
        }

        public virtual void StopSupply()
        {
            _isEnergySuppplied = false;
        }

        private void Update()
        {
            if (ShouldFire)
            {
                AimTarget();
            }
        }

        protected virtual void CheckAndShoot()
        {
            if (!isEnable) return;
            _target = _targetQueue.FirstOrDefault()?.transform;
            if (CanFire && ShouldFire)
            {
                Shoot();
            }
        }

        protected abstract void AimTarget();

        protected abstract void Shoot();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                _targetQueue.Add(other);
            }
            // for testing
            // if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            // {
            //     _targetQueue.Add(other);
            // }
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