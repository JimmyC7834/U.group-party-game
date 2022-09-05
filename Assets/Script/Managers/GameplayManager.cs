using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class GameplayManager : MonoBehaviour
    {
        [SerializeField] private GameplayService _gameplayService;
        
        [SerializeField] private EnemyManager _enemyManager;
        [SerializeField] private ResourceManager _resourceManager;

        private void Awake()
        {
            _gameplayService.ProvideEnemyManager(_enemyManager);
            _gameplayService.ProvideResourceManager(_resourceManager);
        }
    }
}