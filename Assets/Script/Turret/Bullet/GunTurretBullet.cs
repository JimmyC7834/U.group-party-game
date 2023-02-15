using UnityEngine;
using Game.Enemy;

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
            Debug.Log("Hit!");
            EnemyStatus status = collider.GetComponent<EnemyStatus>();
            status?.Hit(_damage);
            ReturnToPool();
        }
    }
}