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
            // Debug.Log("Hit!");
            EnemyStats enemyStats = collider.GetComponent<EnemyStats>();
            enemyStats?.Hit(_damage);
            // for testing
            // PlayerStats playerStats = collider.GetComponent<PlayerStats>();
            // playerStats?.Hit(_damage);
            ReturnToPool();
        }
    }
}