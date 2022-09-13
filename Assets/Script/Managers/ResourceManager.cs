using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Dataset;
using Game.Resource;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;
        [SerializeField] private ResourceDataset _resourceDataset;
        
        private ObjectPool<ResourceObject> _pool;
        [SerializeField] private ResourceObject _prefab;
        
        private void Awake()
        {
            _pool = new ObjectPool<ResourceObject>(
                CreateResourceObject,
                PoolResourceObject,
                ReturnResourceObject
            );
            
        }

        private ResourceObject CreateResourceObject()
        {
            ResourceObject newObject = Instantiate(_prefab).GetComponent<ResourceObject>();
            newObject.SetPool(_pool);
            return newObject;
        }
        private void PoolResourceObject(ResourceObject resourceObject) => resourceObject.gameObject.SetActive(true);
        private void ReturnResourceObject(ResourceObject resourceObject) => resourceObject.gameObject.SetActive(false);

        
        public ResourceObject SpawnResource(ResourceId id)
        {
            ResourceObject resourceObject = _pool.Get();
            resourceObject.Initialize(_resourceDataset[id]);
            return resourceObject;
        }
    }
}