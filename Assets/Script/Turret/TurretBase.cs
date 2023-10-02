using System.Collections.Generic;
using System.Linq;
using Game.Core;
using UnityEngine;

namespace Game.Turret
{
    [RequireComponent(typeof(Timer))]
    [RequireComponent(typeof(Throwable))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(TargetLoader))]

    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected Transform _shootingPoint;
        [SerializeField] protected Transform _partToRotate;
        [SerializeField] protected Transform _target;
        [SerializeField] protected string _targetLayer = "Enemy";
        protected BulletManager _bulletManager;

        [Space]
        [Header("Turret Data")]
        [SerializeField] private float _cooldownInterval; // unit: ms
        [SerializeField] protected float _shootingRange = 3f;
        [SerializeField] protected float _rotateSpeed = 5f;
        [SerializeField] protected float _durability = 100f;
        [SerializeField] protected float _consumptionPerBullet = 5f;
        [SerializeField] protected GameObject _bulletPrefab;


        protected Throwable _throwable;
        protected Timer _timer;
        protected Health _health;
        protected TargetLoader _targetLoader;


        public bool isEnable { get; private set; }
        protected bool ShouldFire => isEnable && _target != null;
        // priority: picked < reborn < has_health

        private void Awake()
        {
            _bulletManager = gameObject.AddComponent<BulletManager>();
            _bulletManager.Initialize(transform, _bulletPrefab);

            // new components
            _throwable = GetComponent<Throwable>();
            _throwable.OnHeld += (_) => Disable();
            _throwable.OnGrounded += Enable;

            _timer = GetComponent<Timer>();
            _timer.SetSec(_cooldownInterval);
            _timer.SetCallBack(CheckAndShoot);

            _health = GetComponent<Health>();
            _health.InitHealth(_durability);
            _health.OnDead += Disable;
            _health.OnDead += () => Debug.Log("Ran out of durability!");
            _health.OnReborn += Enable;

            _targetLoader = GetComponent<TargetLoader>();
            _targetLoader.SetRange(_shootingRange);
            _targetLoader.SetLayer(_targetLayer);

            Enable();
        }

        private void Update()
        {
            if (ShouldFire)
            {
                AimTarget();
            }
        }

        protected virtual void Disable()
        {
            if (!isEnable) return;
            Debug.Log("Disabled");
            isEnable = false;
            // _timer.Stop();
        }

        protected virtual void Enable()
        {
            if (isEnable) return;
            Debug.Log("Enabled");
            isEnable = true;
            _timer.Time();
        }

        protected virtual void CheckAndShoot()
        {
            // Debug.Log("check and shoot");
            if (!isEnable) return;
            _target = _targetLoader.GetTarget();
            // Debug.Log(ShouldFire);
            if (ShouldFire)
            {
                Shoot();
                _health.Damage(_consumptionPerBullet);
            }
            _timer.Time();
        }

        protected abstract void AimTarget();
        protected abstract void Shoot();

    }
}