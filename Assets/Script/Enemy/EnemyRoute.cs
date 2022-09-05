using Game.Data;
using UnityEngine;
using UnityEngine.Pool;

namespace Game.Enemy
{
    public class EnemyRoute : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;
        
        private ObjectPool<EnemyController> _pool;
        [SerializeField] private EnemyController _prefab;

        public EnemyRouteNode[] nodes;

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
        
        private void OnValidate()
        {
            nodes = GetComponentsInChildren<EnemyRouteNode>();
        }
        
        public void SpawnEnemy(EnemyId id)
        {
            EnemyController enemy = _gameplayService.enemyManager.SpawnEnemy(id);
            enemy.transform.position = transform.position;
            enemy.nextPoint = nodes[0].transform;
        }
    }
}