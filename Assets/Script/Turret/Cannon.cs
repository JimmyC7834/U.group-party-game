using UnityEngine;

namespace Game.Turret
{
    public class Cannon : TurretBase
    {
        protected override void Shoot()
        {
            TurretBulletBase bullet = _bulletManager.SpawnBullet();
            bullet.Initialize(_shootingPoint.position, _partToRotate.rotation, _target);
        }

        // not working precisely, to be fixed
        protected override void AimTarget()
        {
            Vector3 dir = (transform.position - _target.transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.forward);
            Vector3 rotation = Quaternion.Slerp(
                _partToRotate.rotation, lookRotation, Time.deltaTime * _rotateSpeed).eulerAngles;
            _partToRotate.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }
    }
}