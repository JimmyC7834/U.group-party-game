using UnityEngine;
using UnityEngine.Pool;

namespace Game.Turret
{
    public class TurretBase : MonoBehaviour
    {
        public Transform shootingPoint;
        public float shootingRange = 3f;
        public Transform target;
        public Transform partToRotate;
        public float rotateSpeed = 5f;
        private bool IsGrounded = true;

        [Space]
        [Header("Bullet Manager")]
        public BulletManager bulletManager;

        // Start is called before the first frame update
        void Start()
        {
            // should be called by event when is grounded
            InvokeRepeating("UpdateTarget", 0f, 0.1f);
            InvokeRepeating("Shoot", 1f, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            IsGrounded = GetComponent<ThrowableObject>().IsGrounded;

            if (target == null || !IsGrounded)
                return;

            AimTarget();
            // An event happen at intervals, just decide turret need to shooting or not
        }

        // not working precisely, to be fixed
        void AimTarget()
        {
            Vector3 dir = transform.position - target.transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.forward);
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotateSpeed)
                .eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }

        virtual public void UpdateTarget()
        {
            // enemyNearbyArray is bounded due to OverlapCircleNonAlloc
            Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, shootingRange, LayerMask.GetMask("Enemy"));
            
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
            target = (nearestEnemy != null) ? nearestEnemy.transform : null;
        }

        virtual public void Shoot()
        {
            if (target == null || !IsGrounded)
                return;

            // gameobject?
            TurretBulletBase bullet = bulletManager.SpawnBullet();
            bullet.Initialize(shootingPoint.position, transform.rotation, target);
            bullet.Activate();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);
        }
    }
}