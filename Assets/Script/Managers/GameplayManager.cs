using Game.Turret;
using UnityEngine;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;

        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private ResourceManager _resourceManager;
        [SerializeField] private BulletManager _bulletManager;

        private void Awake()
        {
            _gameplayService.ProvideEnemyManager(_enemyManager);
            _gameplayService.ProvideResourceManager(_resourceManager);
            _gameplayService.ProvideBulletManager(_bulletManager);
        }
    }
}