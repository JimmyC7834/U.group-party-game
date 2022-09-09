using Game.Turret;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Service/Gameplay", fileName = "GameplayService")]
    public class GameplayService : ScriptableObject
    {
        public EnemyManager enemyManager { get; private set; }
        public ResourceManager resourceManager { get; private set; }
        public BulletManager bulletManager { get; private set; }

        public void ProvideEnemyManager(EnemyManager _enemyManager) => enemyManager = _enemyManager;
        public void ProvideResourceManager(ResourceManager _resourceManager) => resourceManager = _resourceManager;
        public void ProvideBulletManager(BulletManager _bulletManager) => bulletManager = _bulletManager;
    }
}