using UnityEngine;

namespace Game.Turret
{
    public class Missile : TurretBulletBase
    {
        private Transform _target;
        private FakeHeightObject _fakeHeightObject;
        private bool _launched = false;
        public float ExplosionRange = 1f;

        private void Awake() {
            _fakeHeightObject = gameObject.GetComponent<FakeHeightObject>();  
            // _fakeHeightObject.OnLaunch += () => _launched = true;
            _fakeHeightObject.OnGrounded += Explode;
        }
        protected override void UpdatePosition()
        {
            // float distanceThisFrame = _speed * Time.deltaTime;
            // transform.Translate(Vector3.up * distanceThisFrame, Space.Self);
            if (_fakeHeightObject.IsGrounded && _launched)
            {
                // Explode();
            }
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
            Collider2D[] enemiesHitten = Physics2D.OverlapCircleAll(transform.position, ExplosionRange, LayerMask.GetMask("Enemy"));
            foreach(Collider2D enemy in enemiesHitten)
            {
                
                // sth like enemy.GetComponent<EnemyData>().hitten(_damage)
            }
            ReturnToPool();
        }

        protected override void Hit(Collider2D collider)
        {
            Debug.Log("Hit!");
        }
    }
}
