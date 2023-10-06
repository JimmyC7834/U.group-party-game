using Game.Data;
using Game.ObjectStatus;
using UnityEngine;

public class PlayerStats : PoolObjectStatsBase
{
    public void Initialize(PlayerSO data)
    {
        isDied = false;
        _strength = data.strength;
        _maxHealth = data.maxHealth;
        _health = _maxHealth;
    }
}
