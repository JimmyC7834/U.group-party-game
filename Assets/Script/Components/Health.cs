using Game.Data;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{
    public class Health : MonoBehaviour
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _minHealth;
        [SerializeField] private float _health;

        public event UnityAction<float> OnHealthChange = delegate { };

        public void SetHealth(float value)
        {
            _health = value;
            OnHealthChange.Invoke(_health);
        }
    }
}