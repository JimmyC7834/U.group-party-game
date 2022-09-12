using UnityEngine;

namespace Game.Turret
{
    public class Missile : TurretBulletBase
    {
        private Transform _target;
        private FakeHeightObject _fakeHeightObject;
        [SerializeField] private float _explosionRange = 1f;

        private void Awake()
        {
            _fakeHeightObject = gameObject.GetComponent<FakeHeightObject>();
            _fakeHeightObject.OnGrounded += Explode;
        }

        public override void Initialize(Vector3 _position, Quaternion _rotation, Transform _target)
        {
            base.Initialize(_position, _rotation);
            Vector3 dir = Vector3.Normalize(_target.position - _position);
            _fakeHeightObject.Launch(dir * _speed, _speed, 0f);
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
    }
}