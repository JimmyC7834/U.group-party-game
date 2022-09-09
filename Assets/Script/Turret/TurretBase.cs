using UnityEngine;

namespace Game.Turret
{
    public abstract class TurretBase : MonoBehaviour
    {
        [SerializeField] protected GameplayService _gameplayService;
        [SerializeField] protected Transform _shootingPoint;
        [SerializeField] protected float _shootingRange = 3f;
        [SerializeField] protected Transform _target;
        [SerializeField] protected Transform _partToRotate;
        [SerializeField] protected float _rotateSpeed = 5f;
        private bool _enable = true;

        private void Awake()
        {
            _partToRotate = transform;
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

        protected abstract void AimTarget();

        protected abstract void UpdateTarget();

        protected abstract void Shoot();

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, _shootingRange);
        }
    }
}