using UnityEngine;

using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Player;
using Fingers.UI.Gameplay;

namespace Fingers.Gameplay.Movement
{
    public class GameplayArea : MonoBehaviour
    {
        [SerializeField] private Transform pointScoring;
        
        private IStaticDataService _staticDataService;
        private GameplayHandler _gameplayHandler;
        private EnemiesArea _enemiesArea;
        private PlayerFinger _playerFinger;

        private Vector3 _startPosition;

        private bool _isPlaying;
        private bool _isLockPlayer;

        public bool IsLockPlayer
        {
            set => _isLockPlayer = value;
        }

        private void Awake()
        {
            _startPosition = transform.position;
        }

        public void Construct(IStaticDataService staticDataService,
            GameplayHandler gameplayHandler,
            EnemiesArea enemiesArea,
            PlayerFinger playerFinger)
        {
            _staticDataService = staticDataService;
            _gameplayHandler = gameplayHandler;
            _enemiesArea = enemiesArea;
            _playerFinger = playerFinger;
        }

        private void Update()
        {
            MovementArea();
        }

        public void Activate()
        {
            if (_isLockPlayer)
                return;
            
            _isPlaying = true;
            
            _enemiesArea.Play();
        }

        public void Deactivate()
        {
            _enemiesArea.Stop();
            
            _isPlaying = false;
            _isLockPlayer = true;
            transform.position = _startPosition;
            _playerFinger.ResetPosition();
        }

        private void MovementArea()
        {
            if (!_isPlaying)
                return;
            
            transform.Translate(0f, -_staticDataService.Gameplay.startSpeedMove * Time.deltaTime, 0f);
            _enemiesArea.Movement(_staticDataService.Gameplay.startSpeedMove * Time.deltaTime);
            _gameplayHandler.UpdateScores(GetScores());
        }

        public void UpdateFingerPosition(in Vector3 newPosition)
        {
            if (_isLockPlayer)
                return;
            
            _playerFinger.UpdatePosition(newPosition);
        }

        private int GetScores()
        {
            return (int) Vector2.Distance(pointScoring.position, _playerFinger.transform.position);
        }
    }
}
