using System.Collections.Generic;
using System.Runtime.InteropServices;
using Fingers.Constants;
using TMPro;
using UnityEngine;

using Fingers.Core.Progress;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;

namespace Fingers.UI.MainMenu
{
    public class ResultScoresView : MonoBehaviour, IReadingProgress
    {
        [DllImport("__Internal")]
        private static extern void TargetMedalExtern(int value);
        
        [SerializeField] private TMP_Text scoreValue;
        [SerializeField] private TMP_Text descriptionValue;
        
        [Space]
        [SerializeField] private List<GameObject> medals;
        [SerializeField] private List<MedalProgress> medalsProgress;
    
        private IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;
        private IProgressProviderService _progressProviderService;

        public void Construct(IStaticDataService staticDataService,
            ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            Register(_progressProviderService);
        
            medals.ForEach(data => data.SetActive(false));
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            descriptionValue.text = progress.ScoresData.RecordNumber > 0
                ? $"{_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_RECORD)} {progress.ScoresData.RecordNumber}"
                : string.Empty;

            UpdateMedals(progress.ScoresData.MedalsProgress);
        }

        public void UpdateProgress(ProgressData progress)
        {
            UpdateScores(progress.ScoresData.LastNumber, progress.ScoresData.DayRecordNumber, progress.ScoresData.RecordNumber);
            UpdateMedals(progress.ScoresData.MedalsProgress);
        }

        private void UpdateScores(int lastNumber, int dayRecordNumber, int recordNumber)
        {
            scoreValue.text = lastNumber.ToString();

            if (lastNumber == recordNumber)
            {
                descriptionValue.text = _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_NEW_RECORD);
            }
            else if (lastNumber == dayRecordNumber)
            {
                descriptionValue.text = _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_NEW_DAY_RECORD);
            }
            else
            {
                if (dayRecordNumber > 0)
                    descriptionValue.text = Random.Range(0,2) == 0 
                        ? $"{_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_DAY_RECORD)} {dayRecordNumber}" 
                        : $"{_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_RECORD)} {recordNumber}";
                else
                    descriptionValue.text = $"{_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_RECORD)} {recordNumber}";
            }

            UpdateMedalIcon(lastNumber);
        }

        private void UpdateMedalIcon(int lastNumber)
        {
            medals.ForEach(data => data.SetActive(false));
        
            for (var i = _staticDataService.Progress.medalsValue.Count - 1; i >= 0; i--)
            {
                if (lastNumber >= _staticDataService.Progress.medalsValue[i])
                {
                    medals[i].SetActive(true);
                    
                    break;
                }
            }
        }

        private void UpdateMedals(List<int> medalsProgress)
        {
            for (var i = 0; i < medalsProgress.Count; i++)
            {
                if (medalsProgress[i] > 0)
                {
                    if (medalsProgress[i] == 1)
                        TargetMedalExtern(i + 1);
                    
                    this.medalsProgress[i].Activate(medalsProgress[i]);
                }
            }
        }
    }
}
