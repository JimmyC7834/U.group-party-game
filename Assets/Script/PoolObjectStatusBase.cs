using Game.Data;
using UnityEngine;
using UnityEngine.Events;


namespace Game.ObjectStatus
{
    public abstract class PoolObjectStatusBase : MonoBehaviour
    {
        [SerializeField] private float health;
        [SerializeField] private float attack;
        private bool isDied = false;
        
        public event UnityAction Killed;

        public void Initialize(EnemySO data)
        {
            isDied = false;
            health = data.health;
            attack = data.attack;
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
            if (health <= 0)
            {
                Kill();
            }
        }

        public virtual void Hit(float damage)
        {
            health -= damage;
            CheckHealth();
        }

        // public void FixedUpdate()
        // {
        //     CheckHealth();
        // }
    }

}
