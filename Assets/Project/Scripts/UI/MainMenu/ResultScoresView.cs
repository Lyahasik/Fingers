using System.Collections.Generic;
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
        [SerializeField] private TMP_Text scoreValue;
        [SerializeField] private TMP_Text descriptionValue;
        [SerializeField] private List<GameObject> medals;
    
        private IStaticDataService _staticDataService;
        private ILocalizationService _localizationService;

        public void Construct(IStaticDataService staticDataService, ILocalizationService localizationService)
        {
            _staticDataService = staticDataService;
            _localizationService = localizationService;
        }

        public void Initialize(IProgressProviderService progressProviderService)
        {
            Register(progressProviderService);
        
            medals.ForEach(data => data.SetActive(false));
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            descriptionValue.text = $"{_localizationService.LocaleMain(ConstantValues.KEY_LOCALE_RECORD)} {progress.ScoresData.RecordNumber}";
        }

        public void UpdateProgress(ProgressData progress)
        {
            UpdateScores(progress.ScoresData.LastNumber, progress.ScoresData.DayRecordNumber, progress.ScoresData.RecordNumber);
        }
    
        private void UpdateScores(int lastNumber, int dayRecordNumber, int recordNumber)
        {
            scoreValue.text = lastNumber.ToString();

            if (lastNumber > recordNumber)
            {
                descriptionValue.text = _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_NEW_RECORD);
            }
            else if (lastNumber > dayRecordNumber)
            {
                descriptionValue.text = _localizationService.LocaleMain(ConstantValues.KEY_LOCALE_DAY_RECORD);
            }
            else
            {
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
    }
}
