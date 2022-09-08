using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Turret
{
    public class TurretBulletBase : MonoBehaviour
    {
        private Transform target;
        private Vector3 dir;
        private ObjectPool<TurretBulletBase> _pool;

        [Header("Bullet Data")]
        // should use a dataform
        [SerializeField] public float damage = 0f;
        [SerializeField] public float speed = 15f;
        [SerializeField] private float destoryTime = 3f;
        private bool isActive = false;


        // Update is called once per frame
        void Update()
        {
            if(!isActive)
                return;

            Travel();
        }

        virtual public void Travel()
        {
            float distanceThisFrame = speed * Time.deltaTime;

            // if(dir.magnitude <= distanceThisFrame)
            // {
            //     HitTarget();
            //     target = null;
            //     return;
            // }

            transform.Translate(Vector3.up * distanceThisFrame, Space.Self);
        }

        public void Initialize(Vector3 _position, Quaternion _rotation, Transform _target)
        {
            transform.position = _position;
            transform.rotation = _rotation;
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
                Invoke("ReturnToPool", destoryTime);
            }
            else
            {
                Debug.Log("No target!");
                ReturnToPool();
            }
            isActive = true;
        }

        public void SetPool(ObjectPool<TurretBulletBase> pool) => _pool = pool;

        public void ReturnToPool() => _pool.Release(this);
    }
}
