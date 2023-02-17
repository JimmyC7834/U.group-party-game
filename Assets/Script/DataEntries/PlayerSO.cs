using System.Collections;
using System.Collections.Generic;
using Game.Dataset;
using TMPro;
using UnityEngine;

namespace Game.Data
{    
    [CreateAssetMenu(menuName = "Game/DataEntry/Player", fileName = "PlayerSO")]
    public class PlayerSO : ScriptableObject
    {
        [SerializeField] private float _maxHealth;
        [SerializeField] private float _strength;
        
        public float maxHealth { get => _maxHealth; }
        public float strength { get => _strength; }
    }
}