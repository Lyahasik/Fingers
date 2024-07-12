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
            _gameplayArea.UpdateFingerPosition(eventData.position);
            _mainMenuHandler.DeactivateMenu();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _gameplayArea.UpdateFingerPosition(eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _gameplayHandler.EndGame();
        }
    }
}
