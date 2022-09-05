using System.Collections;
using System.Collections.Generic;
using Game.Data;
using Game.Dataset;
using UnityEngine;
using UnityEngine.Pool;

using Game.Enemy;

namespace Game
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;
        [SerializeField] private EnemyDataset _enemyDataset;
        
        private ObjectPool<EnemyController> _pool;
        [SerializeField] private EnemyController _prefab;
        
        private void Awake()
        {
            _pool = new ObjectPool<EnemyController>(
                CreateEnemyController,
                PoolEnemyController,
                ReturnEnemyController
            );
            
        }

        private EnemyController CreateEnemyController() => Instantiate(_prefab).GetComponent<EnemyController>();
        private void PoolEnemyController(EnemyController enemyController) => enemyController.gameObject.SetActive(true);
        private void ReturnEnemyController(EnemyController enemyController) => enemyController.gameObject.SetActive(false);

        
        public EnemyController SpawnEnemy(EnemyId id)
        {
            EnemyController enemy = _pool.Get();
            enemy.Initialize(_enemyDataset[id]);
            return enemy;
        }
    }
}