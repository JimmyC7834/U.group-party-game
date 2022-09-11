using UnityEngine;

namespace Game.Turret
{
    public class Cannon : TurretBase
    {
        public Cannon()
        {
            _cooldown = 3f;
            _shootingRange = 8f;
            _consumptionPerBullet = 20f;
        }
        private void Start()
        {
            // should be called by event when is grounded
            InvokeRepeating("UpdateTarget", 0f, 0.1f);
            InvokeRepeating("Shoot", 1f, _cooldown);
            
        }

        protected override void Shoot()
        {
            if (!(IsEnable() && _target != null)) return;
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

            if (!(IsEnable() && _target != null)) return;
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
            _target = null;

            _target = (_targetQueue.Count > 0) ? _targetQueue[0].transform : null;
        }
    }
}
