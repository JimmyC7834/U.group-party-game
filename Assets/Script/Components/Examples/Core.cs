using System.Collections;
using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health))]
public class Core : MonoBehaviour
{
    [SerializeField] private float _maxCoreHealth = 1000f;
    private Health _health;

    public event UnityAction OnDefeat = delegate { };

    private void Awake()
    {
        _health = GetComponent<Health>();
        _health.InitHealth(_maxCoreHealth);
        _health.OnDead += KilledByEnemy;
    }

    private void KilledByEnemy()
    {
        OnDefeat.Invoke();
    }
}
