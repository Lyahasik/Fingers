using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.StaticData;
using UnityEngine;

using Fingers.Gameplay.Player;
using Fingers.UI.Gameplay;
using UnityEngine.UI;

namespace Fingers.Gameplay.Movement
{
    public class GameplayArea : MonoBehaviour
    {
        [SerializeField] private Image background1;
        [SerializeField] private Image background2;
        [SerializeField] private Transform pointScoring;

        private IStaticDataService _staticDataService;
        private GameplayHandler _gameplayHandler;
        private EnemiesArea _enemiesArea;
        private PlayerFinger _playerFinger;

        private Vector3 _startPosition;
        private Vector3 _startPositionBackground;
        private float _distanceBackground;
        private int _difficultyId;

        private bool _isPlaying;
        private bool _isLockPlayer;

        public bool IsLockPlayer
        {
            get => _isLockPlayer;
            set => _isLockPlayer = value;
        }

        private void Awake()
        {
            _startPosition = transform.position;
            
            _startPositionBackground = background1.transform.position;
            _distanceBackground = Vector3.Distance(_startPositionBackground, background2.transform.position);
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

        public void Pause()
        {
            _enemiesArea.Pause();

            _isPlaying = false;
            _isLockPlayer = true;
            _playerFinger.ResetPosition();
        }

        public void Deactivate()
        {
            _enemiesArea.Stop();
            
            _isPlaying = false;
            _isLockPlayer = true;
            transform.position = _startPosition;
            _playerFinger.ResetPosition();
            
            background1.transform.position = _startPosition;
            background2.transform.position = _startPosition + Vector3.up * _distanceBackground;

            _difficultyId = 0;
            UpdateBackgroundSprites();
        }

        public void UpdateFingerPosition(in Vector3 newPosition)
        {
            if (_isLockPlayer)
                return;
            
            _playerFinger.UpdatePosition(newPosition);
        }

        private void MovementArea()
        {
            if (!_isPlaying)
                return;
            
            transform.Translate(0f, -_enemiesArea.DifficultyStaticData.speedMove * Time.deltaTime, 0f);
            _enemiesArea.Movement(_enemiesArea.DifficultyStaticData.speedMove * Time.deltaTime);
            int scores = GetScores();
            _gameplayHandler.UpdateScores(GetScores());
            
            _enemiesArea.TryUpdateDifficulty(scores);
            TryUpdateBackground();
            
            CheckHit();
        }

        private int GetScores()
        {
            return (int) Vector2.Distance(pointScoring.position, _playerFinger.transform.position);
        }

        private void TryUpdateBackground()
        {
            if (!(Vector3.Distance(background1.transform.position, _startPositionBackground) >
                  _distanceBackground)) return;

            _difficultyId++;
            
            background1.transform.position += Vector3.up * _distanceBackground;
            background2.transform.position += Vector3.up * _distanceBackground;
                
            UpdateBackgroundSprites();
        }

        private void UpdateBackgroundSprites()
        {
            int modId = _difficultyId % _staticDataService.Gameplay.difficulties.Count;
            int nextModId = (modId + 1) % _staticDataService.Gameplay.difficulties.Count;
            
            background1.sprite = _staticDataService.Gameplay.difficulties[modId].background;
            background2.sprite = _staticDataService.Gameplay.difficulties[nextModId].background;
        }

        private void CheckHit()
        {
            RaycastHit hit;
            if (Physics.SphereCast(_playerFinger.Position, _staticDataService.Gameplay.playerRadius, Vector3.forward, out hit))
                _gameplayHandler.ChangeState<GameplayPauseState>();
        }
    }
}
