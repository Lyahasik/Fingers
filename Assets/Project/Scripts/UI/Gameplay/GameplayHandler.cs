using Fingers.Core.Progress;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using Fingers.Gameplay.Movement;
using Fingers.UI.Hud;
using Fingers.UI.MainMenu;
using UnityEngine;

namespace Fingers.UI.Gameplay
{
    public class GameplayHandler : MonoBehaviour, IWritingProgress
    {
        [SerializeField] private ActiveArea activeArea;
        [SerializeField] private GameplayArea gameplayArea;

        private IProgressProviderService _progressProviderService;
        private MainMenuHandler _mainMenuHandler;
        private HudView _hudView;

        private bool _isGameActive;
        private int _scores;

        public bool IsGameActive => _isGameActive;

        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Construct(IProgressProviderService progressProviderService, MainMenuHandler mainMenuHandler, HudView hudView)
        {
            _progressProviderService = progressProviderService;
            _mainMenuHandler = mainMenuHandler;
            _hudView = hudView;
        }

        public void Initialize(IStaticDataService staticDataService)
        {
            gameplayArea.Construct(staticDataService, this);
            activeArea.Construct(staticDataService, _mainMenuHandler, this, gameplayArea);
            
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

        public void StartGame()
        {
            _isGameActive = true;
            gameplayArea.Activate();
            
            Debug.Log("Start game");
        }

        public void EndGame()
        {
            WriteProgress();
                
            _isGameActive = false;
            UpdateScores(0);
                
            gameplayArea.Deactivate();
            _mainMenuHandler.ActivateMenu();
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
