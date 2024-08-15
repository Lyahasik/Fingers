using UnityEngine;

using Fingers.Core.Progress;
using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.GameStateMachine.States;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.Core.Update;
using Fingers.Gameplay.Movement;
using Fingers.Gameplay.Player;
using Fingers.UI.Hud;
using Fingers.UI.MainMenu;

namespace Fingers.UI.Gameplay
{
    public class GameplayHandler : MonoBehaviour, IWritingProgress
    {
        [SerializeField] private ActiveArea activeArea;
        [SerializeField] private GameplayArea gameplayArea;

        private IProgressProviderService _progressProviderService;
        private GameplayStateMachine _gameplayStateMachine;
        private MainMenuHandler _mainMenuHandler;
        private HudView _hudView;
        
        private int _scores;

        public IState ActiveState => _gameplayStateMachine.ActiveState;
        public GameplayArea GameplayArea => gameplayArea;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Construct(IProgressProviderService progressProviderService,
            MainMenuHandler mainMenuHandler,
            HudView hudView)
        {
            _progressProviderService = progressProviderService;
            _mainMenuHandler = mainMenuHandler;
            _hudView = hudView;
        }

        public void Initialize(IStaticDataService staticDataService,
            UpdateHandler updateHandler,
            EnemiesArea enemiesArea,
            PlayerFinger playerFinger)
        {
            gameplayArea.Construct(staticDataService, this, enemiesArea, playerFinger);
            activeArea.Construct(staticDataService, _mainMenuHandler, this, gameplayArea);

            _gameplayStateMachine = new GameplayStateMachine();
            _gameplayStateMachine.Initialize(staticDataService, updateHandler, this);
            
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress) {}

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            UpdateRecord();
            
            _progressProviderService.SaveProgress();
        }

        public void ChangeState<TState>() where TState : class, IState
        {
            if (typeof(TState) == typeof(GameplayActiveState))
            {
                if (_gameplayStateMachine.ActiveState is not GameplayPauseState
                    && _gameplayStateMachine.ActiveState is not GameplayPrepareState)
                {
                    _gameplayStateMachine.Enter<GameplayPrepareState>();
                    
                    return;
                }
            }
            else if (typeof(TState) == typeof(GameplayInactiveState)
                     && _gameplayStateMachine.ActiveState is GameplayPrepareState)
            {
                return;
            }

            _gameplayStateMachine.Enter<TState>();
        }

        public void StartGame()
        {
            gameplayArea.Activate();
        }

        public void PauseGame()
        {
            gameplayArea.Pause();
            _mainMenuHandler.PauseGame(_scores);
        }

        public void EndGame()
        {
            WriteProgress();

            _mainMenuHandler.ActivateMenu(_scores);
            gameplayArea.Deactivate();
            
            UpdateScores(0);
        }

        public void UpdateScores(int scores)
        {
            if (scores != 0
                && _scores >= scores)
                return;
            
            _scores = scores;
            _hudView.UpdateScores(_scores);
        }

        private void UpdateRecord()
        {
            _progressProviderService.ProgressData.ScoresData.LastNumber = _scores;
                
            if (_progressProviderService.ProgressData.ScoresData.DayRecordNumber < _scores)
                _progressProviderService.ProgressData.ScoresData.DayRecordNumber = _scores;
                
            if (_progressProviderService.ProgressData.ScoresData.RecordNumber < _scores)
                _progressProviderService.ProgressData.ScoresData.RecordNumber = _scores;
        }
    }
}
