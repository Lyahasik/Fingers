using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Movement;
using Fingers.UI.MainMenu;
using UndergroundFortress.Extensions;
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
        
        private Camera _mainCamera;
        private RectTransform _rectTransform;

        private void Start()
        {
            _mainCamera = Camera.main;
            _rectTransform = GetComponent<RectTransform>();
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

        public void OnPointerDown(PointerEventData eventData)
        {
            _gameplayArea.IsLockPlayer = false;

            _gameplayArea.UpdateFingerPosition(_mainCamera.ScreenToWorldPoint(eventData.position));
            
            _gameplayHandler.PreparePromptEnemy();
            _gameplayHandler.ChangeState<GameplayActiveState>();
            
            if (_gameplayHandler.ActiveState is GameplayPrepareState)
                _mainMenuHandler.DeactivateMenu();
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 worldPosition = _mainCamera.ScreenToWorldPoint(eventData.position);
            
            _gameplayArea.UpdateFingerPosition(_rectTransform.KeepPointInBoundaries(worldPosition));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _gameplayArea.IsLockPlayer = true;
            
            if (_gameplayHandler.ActiveState is not GameplayPauseState
                && _gameplayHandler.ActiveState is not GameplayPrepareState)
                _gameplayHandler.ChangeState<GameplayInactiveState>();
            
            if (_gameplayHandler.ActiveState is GameplayPrepareState)
                _gameplayHandler.PreparePromptStart();
        }
    }
}
