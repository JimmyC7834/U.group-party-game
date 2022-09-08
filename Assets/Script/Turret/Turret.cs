using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Game;

namespace Game.Turret
{
    public class Turret : MonoBehaviour
    {
        public GameObject bulletPrefab = null;
        public Transform shootingPoint;
        public float shootingRange = 3f;
        public string enemyTag = "Enemy";
        public Transform target = null;
        public Transform partToRotate = null;
        public float rotateSpeed = 5f;
        private bool IsGrounded = true;


        // Start is called before the first frame update
        void Start()
        {
            // should be called by event when is grounded
            InvokeRepeating("updateTarget", 0f, 0.1f);
            InvokeRepeating("Shoot", 1f, 0.5f);
        }

        // Update is called once per frame
        void Update()
        {
            IsGrounded = this.GetComponent<ThrowableObject>().IsGrounded;

            if (target == null || !IsGrounded)
                return;

            AimTarget();
            // An event happen at intervals, just decide turret need to shooting or not
        }

        void updateTarget()
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
            Vector3 rotation = Quaternion.Lerp(partToRotate.rotation, lookRotation, Time.deltaTime * rotateSpeed).eulerAngles;
            partToRotate.rotation = Quaternion.Euler(0f, 0f, rotation.z);
        }

        void Shoot()
        {
            if(target == null || !IsGrounded)
                return;


            // y-axis facing, bug to be fixed
            // Vector3 dir =  target.position - shootingPoint.position;
            // Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
            // Vector3 rotation = lookRotation.eulerAngles;
            // transform.rotation = Quaternion.Euler(0f, 0f, rotation.x + 90);

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

