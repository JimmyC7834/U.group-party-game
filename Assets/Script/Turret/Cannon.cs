using System.Linq;
using UnityEngine;

namespace Game.Turret
{
    public class Cannon : TurretBase
    {
        private void Start()
        {
            // should be called by event when is grounded
            InvokeRepeating("Shoot", 1f, _cooldown);
        }

        protected override void Shoot()
        {
            if (!ShouldFire) return;
            TurretBulletBase bullet = _bulletManager.SpawnBullet();
            bullet.Initialize(_shootingPoint.position, transform.rotation, _target);
            _durability -= _consumptionPerBullet;
        }

        // Update is called once per frame
        private void Update()
        {
            if (_durability <= 0)
            {
                Disable();
            }
            else if (_durability > _consumptionPerBullet)
            {
                Enable();
            }

            _target = _targetQueue.FirstOrDefault()?.transform;
            if (!ShouldFire) return;
            AimTarget();
        }

        // not working precisely, to be fixed
        protected override void AimTarget()
        {
            Vector3 dir = transform.position - _target.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.forward);
            Vector3 rotation = Quaternion.Lerp(
                _partToRotate.rotation, lookRotation, Time.deltaTime * _rotateSpeed).eulerAngles;
            _partToRotate.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }
    }
}