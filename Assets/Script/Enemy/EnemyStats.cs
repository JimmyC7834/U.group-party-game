using Game.ObjectStatus;
using Game.Data;
using UnityEngine;

public class EnemyStats : PoolObjectStatsBase
{
    public void Initialize(EnemySO data)
    {
        isDied = false;
        _strength = data.strength;
        _maxHealth = data.maxHealth;
        _health = _maxHealth;
    }
}
