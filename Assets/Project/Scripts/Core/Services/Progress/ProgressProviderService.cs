using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fingers.Constants;
using Fingers.Core.Progress;
using Fingers.Core.Publish;
using Fingers.Core.Publish.Services.Analytics;
using Fingers.Core.Services.GameStateMachine;
using Fingers.Core.Services.GameStateMachine.States;
using Fingers.Core.Services.Scene;
using Fingers.Core.Services.StaticData;
using Fingers.Core.Update;
using Fingers.Gameplay.Scores;
using Fingers.Gameplay.Wallet;
using Fingers.Helpers;
using Newtonsoft.Json;
using UnityEngine;

namespace Fingers.Core.Services.Progress
{
    public class ProgressProviderService : IProgressProviderService, IUpdating
    {
        [DllImport("__Internal")]
        private static extern void LoadedExtern();

        private readonly IGameStateMachine gameStateMachine;
        private readonly IStaticDataService staticDataService;
        private readonly PublishHandler publishHandler;
        private readonly IProcessingAnalyticsService processingAnalyticsService;

        private ISceneProviderService _sceneProviderService;

        private ProgressData _progressData;

        private List<IReadingProgress> _progressReaders;
        private List<IWritingProgress> _progressWriters;

        private bool _isLocalData;

        private bool _isWasChange;
        private float _waitingSavingTime;

        public ISceneProviderService SceneProviderService
        {
            set => _sceneProviderService = value;
        }
        public ProgressData ProgressData => _progressData;

        public ProgressProviderService(
            IGameStateMachine gameStateMachine,
            IStaticDataService staticDataService,
            PublishHandler publishHandler,
            IProcessingAnalyticsService processingAnalyticsService)
        {
            this.gameStateMachine = gameStateMachine;
            this.staticDataService = staticDataService;
            this.publishHandler = publishHandler;
            this.processingAnalyticsService = processingAnalyticsService;
        }

        public void Initialize(UpdateHandler updateHandler)
        {
            _progressReaders ??= new List<IReadingProgress>();
            _progressWriters ??= new List<IWritingProgress>();
            
            updateHandler.AddUpdatedObject(this);
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void Update()
        {
            RegularSave();
        }

        public void StartLoadData()
        {
            if (OSManager.IsEditor())
                LoadProgress(ConstantValues.KEY_LOCAL_PROGRESS);
            else
                publishHandler.StartLoadData();
        }

        public void LoadProgress(string json)
        {
            _isLocalData = json == ConstantValues.KEY_LOCAL_PROGRESS;

            if (_isLocalData)
            {
                _progressData = LoadData(PlayerPrefs.GetString(ConstantValues.KEY_LOCAL_PROGRESS));
            }
            else
            {
                var localProgressData = LoadData(PlayerPrefs.GetString(ConstantValues.KEY_LOCAL_PROGRESS));
                var serverProgressData = LoadData(json);
                _progressData = localProgressData > serverProgressData ? localProgressData : serverProgressData;
            }
            _progressData ??= CreateNewProgress();

            ResetDayProgress();
            
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.LoadProgress(_progressData);
            
            _waitingSavingTime = ConstantValues.DELAY_SAVING;
            
            if (!OSManager.IsEditor())
                LoadedExtern();
            
            Debug.Log("Loaded progress.");
            gameStateMachine.Enter<LoadSceneState>();
            
            _sceneProviderService.LoadLevelScene();
        }

        public void SaveProgress()
        {
            if (_progressData == null)
                return;
            
            UpdateProgress();
            
            string json = JsonConvert.SerializeObject(_progressData, new JsonSerializerSettings());
            PlayerPrefs.SetString(ConstantValues.KEY_LOCAL_PROGRESS, json);
            
            if (!_isLocalData)
                publishHandler.SaveData(json);
            
            _waitingSavingTime = ConstantValues.DELAY_SAVING;
        }

        public void Register(IReadingProgress progressReader)
        {
            _progressReaders.Add(progressReader);
            
            if (_progressData != null)
                progressReader.LoadProgress(ProgressData);
        }

        public void Register(IWritingProgress progressWriter)
        {
            Register(progressWriter as IReadingProgress);

            _progressWriters.Add(progressWriter);
        }

        public void Unregister(IReadingProgress progressReader)
        {
            _progressReaders.Remove(progressReader);
        }

        public void Unregister(IWritingProgress progressWriter)
        {
            Unregister(progressWriter as IReadingProgress);

            _progressWriters.Remove(progressWriter);
        }

        public void WasChange()
        {
            _isWasChange = true;
        }

        public void SetLocale(int localeId)
        {
            _progressData.LocaleId = localeId;
            SaveProgress();
        }

        private void ResetDayProgress()
        {
            int currentDay = DateTime.Now.DayOfYear;

            if (_progressData.LastDayPlaying != currentDay) 
                _progressData.ScoresData.DayRecordNumber = 0;

            _progressData.LastDayPlaying = currentDay;
        }

        private void RegularSave()
        {
            _progressData.TimeGame += Time.deltaTime;
            _waitingSavingTime -= Time.deltaTime;
            
            if (!_isWasChange
                || _waitingSavingTime > 0)
                return;
            
            SaveProgress();
            _isWasChange = false;
        }

        private ProgressData LoadData(string json)
        {
            ProgressData progressData = null;
                
            if (json != null)
                progressData = JsonConvert.DeserializeObject<ProgressData>(json);

            return progressData;
        }

        private ProgressData CreateNewProgress()
        {
            ProgressData progressData = new ProgressData
            {
                TimeGame = 0f,
                
                Wallet = new WalletData(staticDataService.StartProgress.money),
                ScoresData = new ScoresData()
            };

            SaveProgress();

            return progressData;
        }

        private void UpdateProgress()
        {
            foreach (IReadingProgress progressReader in _progressReaders)
                progressReader.UpdateProgress(_progressData);
        }
        
        public void Clear()
        {
            if (!_isLocalData)
                publishHandler.SaveData(JsonConvert.SerializeObject(new ProgressData(), new JsonSerializerSettings()));
            
            PlayerPrefs.DeleteAll();
        }
    }
}