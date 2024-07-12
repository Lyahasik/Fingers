using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Movement;
using Fingers.UI.MainMenu;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Fingers.UI.Gameplay
{
    public class ActiveArea : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
    {
        private IStaticDataService _staticDataService;
        private MainMenuHandler _mainMenuHandler;
        private GameplayHandler _gameplayHandler;
        private GameplayArea _gameplayArea;

        private bool _isDragActive;
        private float _startGameTime;

        private void Update()
        {
            TryStartGame();
        }

        public void Construct(IStaticDataService staticDataService,
            MainMenuHandler mainMenuHandler,
            GameplayHandler gameplayHandler,
            GameplayArea gameplayArea)
        {
            _staticDataService = staticDataService;
            _mainMenuHandler = mainMenuHandler;
            _gameplayHandler = gameplayHandler;
            _gameplayArea = gameplayArea;
        }

        private void TryStartGame()
        {
            if (_startGameTime == 0f
                || _gameplayHandler.IsGameActive
                || !_isDragActive
                || _startGameTime > Time.time)
                return;
            
            _gameplayHandler.StartGame();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            _isDragActive = true;

            if (_startGameTime == 0f)
            {
                _gameplayArea.UpdateFingerPosition(eventData.position);
                
                _startGameTime = Time.time + _staticDataService.Gameplay.delayToStartGame;
                _mainMenuHandler.DeactivateMenu();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _gameplayArea.UpdateFingerPosition(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _isDragActive = false;

            if (_startGameTime <= Time.time)
            {
                _gameplayHandler.EndGame();
                _startGameTime = 0f;
            }
        }
    }
}
