using UnityEngine;

using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Player;
using Fingers.UI.Gameplay;

namespace Fingers.Gameplay.Movement
{
    public class GameplayArea : MonoBehaviour
    {
        [SerializeField] private Transform pointScoring;
        [SerializeField] private PlayerFinger playerFinger;
        
        private IStaticDataService _staticDataService;
        private GameplayHandler _gameplayHandler;
        private EnemiesArea _enemiesArea;
        
        private Vector3 _startPosition;

        private bool _isMovement;

        private void Awake()
        {
            _startPosition = transform.position;
        }

        public void Construct(IStaticDataService staticDataService,
            GameplayHandler gameplayHandler,
            EnemiesArea enemiesArea)
        {
            _staticDataService = staticDataService;
            _gameplayHandler = gameplayHandler;
            _enemiesArea = enemiesArea;
        }

        private void Update()
        {
            MovementArea();
        }

        public void Activate()
        {
            _isMovement = true;
            
            _enemiesArea.Play();
        }

        public void Deactivate()
        {
            _enemiesArea.Stop();
            
            _isMovement = false;
            transform.position = _startPosition;
            playerFinger.ResetPosition();
        }

        private void MovementArea()
        {
            if (!_isMovement)
                return;
            
            transform.Translate(0f, -_staticDataService.Gameplay.startSpeedMove * Time.deltaTime, 0f);
            _enemiesArea.Movement(_staticDataService.Gameplay.startSpeedMove * Time.deltaTime);
            _gameplayHandler.UpdateScores(GetScores());
        }

        public void UpdateFingerPosition(in Vector3 newPosition)
        {
            playerFinger.UpdatePosition(newPosition);
        }

        private int GetScores()
        {
            return (int) Vector3.Distance(pointScoring.position, playerFinger.transform.position);
        }
    }
}
