using UnityEngine;

namespace Game.Turret
{
    public class Turret : MonoBehaviour
    {
        public GameObject bulletPrefab;
        public Transform shootingPoint;
        public float shootingRange = 3f;
        public string enemyTag = "Enemy";
        public Transform target;
        public Transform partToRotate;
        public float rotateSpeed = 5f;
        private bool IsGrounded = true;


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

        void UpdateTarget()
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            float minDis = Mathf.Infinity;
            GameObject nearestEnemy = null;

            foreach (GameObject enemy in enemies)
            {
                float dis = Vector3.Distance(transform.position, enemy.transform.position);
                if (dis < minDis)
                {
                    minDis = dis;
                    nearestEnemy = enemy;
                }
            }

            if (minDis < shootingRange && nearestEnemy != null)
            {
                target = nearestEnemy.transform;
            }
            else
            {
                target = null;
            }
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

        void Shoot()
        {
            if (target == null || !IsGrounded)
                return;


            // y-axis facing, bug to be fixed
            GameObject bullet = Instantiate<GameObject>(bulletPrefab, shootingPoint.position, transform.rotation);
            bullet.GetComponent<TurretBullet>()?.SetTarget(target);
            bullet.GetComponent<TurretBullet>()?.Activate();
        }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, shootingRange);
        }
    }
}