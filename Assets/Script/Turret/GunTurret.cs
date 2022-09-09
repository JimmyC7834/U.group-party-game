using UnityEngine;

namespace Game.Turret
{
    public class GunTurret : TurretBase
    {
        private void Start()
        {
            // should be called by event when is grounded
            InvokeRepeating("UpdateTarget", 0f, 0.1f);
            InvokeRepeating("Shoot", 1f, 0.5f);
        }

        protected override void Shoot()
        {
            TurretBulletBase bullet = _gameplayService.bulletManager.SpawnBullet();
            bullet.Initialize(_shootingPoint.position, transform.rotation);
        }

        // Update is called once per frame
        private void Update()
        {
            if (!enabled) return;

            AimTarget();
        }

        // not working precisely, to be fixed
        protected override void AimTarget()
        {
            Vector3 dir = transform.position - _target.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.forward);
            Vector3 rotation = Quaternion.Lerp(_partToRotate.rotation, lookRotation, Time.deltaTime * _rotateSpeed)
                .eulerAngles;
            _partToRotate.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }

        protected override void UpdateTarget()
        {
            // enemyNearbyArray is bounded due to OverlapCircleNonAlloc
            Collider2D[] enemies =
                Physics2D.OverlapCircleAll(transform.position, _shootingRange, LayerMask.GetMask("Enemy"));

            float minDis = Mathf.Infinity;
            Collider2D nearestEnemy = null;

            foreach (Collider2D enemy in enemies)
            {
                float dis = Vector3.Distance(transform.position, enemy.transform.position);
                if (dis < minDis)
                {
                    minDis = dis;
                    nearestEnemy = enemy;
                }
            }

            _target = (nearestEnemy != null) ? nearestEnemy.transform : null;
        }
    }
}