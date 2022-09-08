using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Turret
{
    public class TurretBullet : MonoBehaviour
    {
        private Transform target;
        private Vector3 dir;

        // should use a dataform
        public float damage = 0f;
        public float speed = 15f;
        private float destoryTime = 3f;
        private bool isActive = false;


        // Update is called once per frame
        void Update()
        {
            if(!isActive)
                return;

            float distanceThisFrame = speed * Time.deltaTime;

            // if(dir.magnitude <= distanceThisFrame)
            // {
            //     HitTarget();
            //     target = null;
            //     return;
            // }

            transform.Translate(Vector3.up * distanceThisFrame, Space.Self);
        }

        public void SetTarget(Transform _target)
        {
            target = _target;
        }

        // should be called only once, waiting for rewrite
        public void Activate()
        {
            if(target != null)
            {
                // dir = target.position - transform.position;
                // Quaternion lookRotation = Quaternion.LookRotation(dir, Vector3.up);
                // Vector3 rotation = lookRotation.eulerAngles;
                // // apply for y-axis, to be fixed
                // // transform.rotation = Quaternion.Euler(rotation);
                // transform.rotation = Quaternion.Euler(dir);
                Destroy(gameObject, destoryTime);
            }
            else
            {
                Debug.Log("No target!");
                Destroy(gameObject, 0);
            }
            isActive = true;
        }
    }
}
