using System.Collections;
using System.Collections.Generic;
using Game.Dataset;
using UnityEngine;

namespace Game.Resource
{
    public enum ResourceId
    {
        Wood,
        Iron,
        Count,
    }
    
    [CreateAssetMenu(menuName = "Game/DataEntry/Resource", fileName = "ResourceSO")]
    public class ResourceSO : DataEntrySO<ResourceId>
    {
        [SerializeField] private Sprite _sprite;
        [SerializeField] private float _weight;
        
        public Sprite sprite { get => _sprite; }
        public float weight { get => _weight; }
    }
}