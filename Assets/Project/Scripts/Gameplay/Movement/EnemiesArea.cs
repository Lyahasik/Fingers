using System.Collections.Generic;
using UnityEngine;

using Fingers.Core.Services.Factories.Gameplay;
using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Enemies;

namespace Fingers.Gameplay.Movement
{
    public class EnemiesArea : MonoBehaviour
    {
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private Transform destroyPoint;
        [SerializeField] private Transform poolPoint;

        private GameplayStaticData _gameplayStaticData;
        private IGameplayFactory _gameplayFactory;

        private int _currentLevelId;
        private List<EnemiesGroup> _poolEnemies;
        private List<EnemiesGroup> _enemies;

        private bool _isReadySpawn;

        public void Construct(GameplayStaticData gameplayStaticData, IGameplayFactory gameplayFactory)
        {
            _gameplayStaticData = gameplayStaticData;
            _gameplayFactory = gameplayFactory;
        }

        public void Initialize()
        {
            _poolEnemies = new List<EnemiesGroup>();
            _enemies = new List<EnemiesGroup>();

            StartLevel(0);
        }

        private void Update()
        {
            TryCreateEnemies();
        }

        public void Play()
        {
            _isReadySpawn = true;
        }

        public void Stop()
        {
            foreach (EnemiesGroup enemiesGroup in _enemies.ToArray()) 
                EnemiesToPool(enemiesGroup);

            _isReadySpawn = false;
        }

        public void StartLevel(int levelId)
        {
            _currentLevelId = levelId;

            if (_poolEnemies.Count > 0)
                foreach (var enemies in _poolEnemies.ToArray())
                    _poolEnemies.Remove(enemies);
            
            _gameplayStaticData.levels[_currentLevelId].enemiesGroups.ForEach(data =>
            {
                EnemiesGroup enemies = _gameplayFactory.CreateEnemiesGroup(data, transform);
                enemies.transform.position = poolPoint.position;
                _poolEnemies.Add(enemies);
            });
        }

        public void TryCreateEnemies()
        {
            if (!_isReadySpawn
                || _poolEnemies.Count == 0)
                return;

            EnemiesGroup enemies = _poolEnemies[Random.Range(0, _poolEnemies.Count)];
            enemies.Activate();
            enemies.transform.position = spawnPoint.transform.position;
            _enemies.Add(enemies);
            _poolEnemies.Remove(enemies);

            _isReadySpawn = false;
        }

        public void Movement(float speedMove)
        {
            foreach (EnemiesGroup enemiesGroup in _enemies.ToArray())
            {
                enemiesGroup.Movement(-speedMove);

                if (!enemiesGroup.IsReady
                    && Vector3.Distance(enemiesGroup.transform.position, spawnPoint.position) > enemiesGroup.Height + _gameplayStaticData.spawnDistance)
                {
                    enemiesGroup.IsReady = true;
                    _isReadySpawn = true;
                }

                if (!(enemiesGroup.TopBorderPoint.position.y < destroyPoint.position.y))
                    continue;
                
                EnemiesToPool(enemiesGroup);
            }
        }

        private void EnemiesToPool(EnemiesGroup enemiesGroup)
        {
            _enemies.Remove(enemiesGroup);
            
            enemiesGroup.Deactivate();
            enemiesGroup.transform.position = poolPoint.position;
            _poolEnemies.Add(enemiesGroup);
        }
    }
}