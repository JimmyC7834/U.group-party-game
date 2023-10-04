using Game.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth = 100f;
        [SerializeField] private float _minHealth = 0f;
        [SerializeField] private float _health;

        public event UnityAction<float> OnHealthChange = delegate { };
        public event UnityAction OnDead = delegate { };
        public event UnityAction OnReborn = delegate { };

        public void InitHealth(float max, float min = 0)
        {
            _maxHealth = max;
            _minHealth = min;
            _health = max;
        }

        public void Damage(float value)
        {
            float health = _health - value;
            if (health < _minHealth) health = _minHealth;
            SetHealth(health);
        }

        public void Cure(float value)
        {
            float health = _health + value;
            if (health > _maxHealth) health = _maxHealth;
            SetHealth(health);
        }

        public void Reborn()
        {
            SetHealth(_maxHealth);
        }

        public void Suicide()
        {
            Damage(_health);
        }

        private void SetHealth(float value)
        {
            _health = value;
            OnHealthChange.Invoke(_health);
            if (_health <= _minHealth)
            {
                OnDead.Invoke();
            }
        }
    }
}