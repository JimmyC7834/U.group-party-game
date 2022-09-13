using Game.Data;
using UnityEngine;

namespace Game.Enemy
{
    public class EnemyRoute : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;

        [SerializeField] private EnemyRouteNode[] nodes;

        private void OnValidate()
        {
            nodes = GetComponentsInChildren<EnemyRouteNode>();
        }

        public void SpawnEnemy(int num) => SpawnEnemy((EnemyId) num);

        public void SpawnEnemy(EnemyId id)
        {
            EnemyController enemy = _gameplayService.enemyManager.SpawnEnemy(id);
            enemy.transform.position = transform.position;
            enemy.RedirectTo(nodes[0]);
        }
    }
}