using UnityEngine;

namespace Game.Turret
{
    public class Missile : TurretBulletBase
    {
        private Transform _target;
        private FakeHeightObject _fakeHeightObject;
        [SerializeField] private float _explosionRange = 1f;
        [SerializeField] private float _travelingTime = 3f;

        [Header("Physics Debug Values")]
        [SerializeField] private float xSpeed = 0;
        [SerializeField] private float ySpeed = 0;
        [SerializeField] private float distance;
        [SerializeField] private float gravity;

        private void Awake()
        {
            _fakeHeightObject = gameObject.GetComponent<FakeHeightObject>();
            _fakeHeightObject.OnGrounded += Explode;
        }

        public override void Initialize(Vector3 _position, Quaternion _rotation, Transform _target)
        {
            base.Initialize(_position, _rotation);
            Vector3 dir = _target.position - _position;
            gravity = Mathf.Abs(_fakeHeightObject.GetGravity());
            distance = dir.magnitude;
            xSpeed = distance / _travelingTime;
            ySpeed = gravity * _travelingTime * 0.5f;
            _fakeHeightObject.Launch(Vector3.Normalize(dir) * xSpeed, ySpeed, 0f);
        }

        private void Explode()
        {
            Debug.Log("Explode!");
            Collider2D[] enemiesHitten =
                Physics2D.OverlapCircleAll(transform.position, _explosionRange, LayerMask.GetMask("Enemy"));
            foreach (Collider2D enemy in enemiesHitten)
            {
                // sth like enemy.GetComponent<EnemyData>().hitten(_damage)
            }

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