using UnityEngine;

namespace Game.Turret
{
    [RequireComponent(typeof(Throwable))]
    public class Missile : TurretBulletBase
    {
        private Throwable _throwable;
        [SerializeField] private float _explosionRange = 1f;
        [SerializeField] private float _travelingTime = 3f;

        [Header("Physics Debug Values")]
        [SerializeField] private float xSpeed = 0;
        [SerializeField] private float ySpeed = 0;
        [SerializeField] private float distance;
        [SerializeField] private float gravity;

        protected override void Awake()
        {
            base.Awake();
            _throwable = GetComponent<Throwable>();
        }

        public override void Initialize(Vector3 _position, Quaternion _rotation, Transform _target)
        {
            base.Initialize(_position, _rotation);
            Vector3 dir = _target.position - _position;
            gravity = Mathf.Abs(_throwable.GetGravity());
            distance = dir.magnitude;
            xSpeed = distance / _travelingTime;
            ySpeed = gravity * _travelingTime * 0.5f;
            _throwable.Launch(Vector3.Normalize(dir) * xSpeed, ySpeed, 0f);
            _throwable.OnGrounded += Explode;
        }

        private void Explode()
        {
            // Debug.Log("Explode!");
            Collider2D[] enemiesHitten =
                Physics2D.OverlapCircleAll(transform.position, _explosionRange, LayerMask.GetMask("Enemy"));
            foreach (Collider2D enemy in enemiesHitten)
            {
                Health health = enemy.GetComponent<Health>();
                health.Damage(_damage);
            }
            /* Bug: Trying to release an object by timer that has already been released to the pool here. */
            ReturnToPool();
        }

        protected override void Hit(Collider2D _)
        {
        }

        protected override void UpdatePosition()
        {
        }
    }
}