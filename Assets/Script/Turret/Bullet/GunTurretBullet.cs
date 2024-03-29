using UnityEngine;
using Game.Enemys;

namespace Game.Turret
{
    public class GunTurretBullet : TurretBulletBase
    {
        protected override void UpdatePosition()
        {
            float distanceThisFrame = _speed * Time.deltaTime;
            transform.Translate(Vector3.up * distanceThisFrame, Space.Self);
        }

        protected override void Hit(Collider2D collider)
        {
            // Debug.Log("Hit!");
            Health health = collider.GetComponent<Health>();
            health.Damage(_damage);
            ReturnToPool();
        }
    }
}