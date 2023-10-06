using Game.Data;
using Game.Dataset;
using Game.Enemys;
using UnityEngine;
using UnityEngine.Pool;

namespace Game
{
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;
        [SerializeField] private EnemyDataset _enemyDataset;

        private ObjectPool<Enemy> _pool;
        [SerializeField] private Enemy _prefab;

        private void Awake()
        {
            _pool = new ObjectPool<Enemy>(
                CreateEnemy,
                PoolEnemy,
                ReturnEnemy
            );
        }

        private Enemy CreateEnemy()
        {
            Enemy newEnemy = Instantiate(_prefab).GetComponent<Enemy>();
            newEnemy.SetPool(_pool);
            return newEnemy;
        }

        private void PoolEnemy(Enemy enemy) => enemy.gameObject.SetActive(true);

        private void ReturnEnemy(Enemy enemy) =>
            enemy.gameObject.SetActive(false);


        public Enemy SpawnEnemy(EnemyId id)
        {
            Enemy enemy = _pool.Get();
            enemy.Initialize(_enemyDataset[id]);
            return enemy;
        }
    }
}