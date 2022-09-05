using System.Collections;
using System.Collections.Generic;
using Game.Enemy;
using UnityEngine;

namespace Game
{
    [CreateAssetMenu(menuName = "Game/Service/Gameplay", fileName = "GameplayService")]
    public class GameplayService : ScriptableObject
    {
        public EnemyManager enemyManager { get; private set; }
        public ResourceManager resourceManager { get; private set; }
        
        public void ProvideEnemyManager(EnemyManager _enemyManager) => enemyManager = _enemyManager;
        public void ProvideResourceManager(ResourceManager _resourceManager) => resourceManager = _resourceManager;
    }
}