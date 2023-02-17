using Game.Dataset;
using System;
using UnityEngine;
using UnityEngine.Events;


namespace Game.ObjectStatus
{
    public abstract class PoolObjectStatsBase : MonoBehaviour
    {
        [SerializeField] protected float _maxHealth;
        [SerializeField] protected float _health;
        [SerializeField] protected float _strength;
        protected bool isDied = false;
        public float health { get => _health; }
        public float strength { get => _strength; }
        
        // Controller listen to status killed event
        public event UnityAction Killed;

        public void Initialize()
        {

        }

        private void Kill()
        {
            Debug.Log("Enemy is died!");
            if (isDied) return;
            isDied = true;
            Killed?.Invoke();
        }

        private void CheckHealth()
        {
            if (_health <= 0)
            {
                Kill();
            }
        }

        public virtual void Hit(float damage)
        {
            _health -= damage;
            CheckHealth();
        }
    }

}
