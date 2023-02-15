using System.Collections;
using System.Collections.Generic;
using Game.Dataset;
using TMPro;
using UnityEngine;

namespace Game.Data
{
    public enum EnemyId
    {
        Slime,
        RedSlime,
    }
    
    [CreateAssetMenu(menuName = "Game/DataEntry/Enemy", fileName = "EnemySO")]
    public class EnemySO : DataEntrySO<EnemyId>
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _speed;
        [SerializeField] private float _health;
        [SerializeField] private float _attack;
        
        public Sprite sprite { get => _sprite; }
        public float speed { get => _speed; }
        public float health { get => _health; }
        public float attack { get => _attack; }
    }
}