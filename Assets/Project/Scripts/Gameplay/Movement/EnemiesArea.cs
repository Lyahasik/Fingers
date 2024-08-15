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

        private DifficultyStaticData _difficultyStaticData;
        private List<EnemiesGroup> _enemies;

        private bool _isReadySpawn;
        private bool _isPause;

        public DifficultyStaticData DifficultyStaticData => _difficultyStaticData;

        public void Construct(GameplayStaticData gameplayStaticData, IGameplayFactory gameplayFactory)
        {
            _gameplayStaticData = gameplayStaticData;
            _gameplayFactory = gameplayFactory;
        }

        public void Initialize()
        {
            _enemies = new List<EnemiesGroup>();

            TryUpdateDifficulty(0);
        }

        private void Update()
        {
            TryCreateEnemies();
        }

        public void Play()
        {
            if (_isPause)
                _isPause = false;
            else
                _isReadySpawn = true;
        }

        public void Pause()
        {
            _isPause = true;
        }

        public void Stop()
        {
            foreach (EnemiesGroup enemiesGroup in _enemies)
                Destroy(enemiesGroup.gameObject);
            
            _enemies.Clear();
            
            _isPause = false;
        }

        public void TryUpdateDifficulty(int scores)
        {
            int difficultyId = 0;
            for (var i = 0; i < _gameplayStaticData.difficulties.Count; i++)
            {
                difficultyId = i;

                if (_gameplayStaticData.difficulties[i].transitionScores > scores)
                    break;
            }
            
            _difficultyStaticData = _gameplayStaticData.difficulties[difficultyId];
        }

        public void TryCreateEnemies()
        {
            if (!_isReadySpawn)
                return;

            var listEnemies = _difficultyStaticData.enemiesGroups;
            EnemiesGroup enemies = _gameplayFactory.CreateEnemiesGroup(listEnemies[Random.Range(0, listEnemies.Count)], transform);
            enemies.Activate();
            enemies.transform.position = spawnPoint.transform.position;
            _enemies.Add(enemies);

            _isReadySpawn = false;
        }

        public void Movement(float speedMove)
        {
            foreach (EnemiesGroup enemiesGroup in _enemies.ToArray())
            {
                enemiesGroup.Movement(-speedMove);

                if (!enemiesGroup.IsReady
                    && Vector3.Distance(enemiesGroup.transform.position, spawnPoint.position) > enemiesGroup.Height + _difficultyStaticData.spawnDistance)
                {
                    enemiesGroup.IsReady = true;
                    _isReadySpawn = true;
                }

                if (!(enemiesGroup.TopBorderPoint.position.y < destroyPoint.position.y))
                    continue;

                _enemies.Remove(enemiesGroup);
                Destroy(enemiesGroup.gameObject);
            }
        }
    }
}