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
        
        private Camera _mainCamera;

        private void Start()
        {
            _mainCamera = Camera.main;
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
            if (_gameplayHandler.IsGameActive)
            {
                _gameplayHandler.StartGame();
            }
            else
                _mainMenuHandler.DeactivateMenu();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _gameplayArea.UpdateFingerPosition(_mainCamera.ScreenToWorldPoint(eventData.position));

            CheckHit(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _gameplayArea.IsLockPlayer = true;
            _gameplayHandler.EndGame();
        }

        private void CheckHit(Vector3 newPosition)
        {
            RaycastHit hit;
            if (Physics.SphereCast(_mainCamera.ScreenToWorldPoint(newPosition), _staticDataService.Gameplay.playerRadius, Vector3.forward, out hit))
                _gameplayHandler.EndGame();
        }
    }
}
