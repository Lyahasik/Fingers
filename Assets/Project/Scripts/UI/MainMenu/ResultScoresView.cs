using System.Collections.Generic;
using Fingers.Core.Progress;
using Fingers.Core.Services.Progress;
using Fingers.Core.Services.StaticData;
using TMPro;
using UnityEngine;

namespace Fingers.UI.MainMenu
{
    public class ResultScoresView : MonoBehaviour, IReadingProgress
    {
        [SerializeField] private TMP_Text scoreValue;
        [SerializeField] private TMP_Text descriptionValue;
        [SerializeField] private List<GameObject> medals;
    
        private IStaticDataService _staticDataService;

        public void Construct(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
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
            descriptionValue.text = "рекорд " + progress.ScoresData.RecordNumber;
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
                descriptionValue.text = "новый рекорд";
            }
            else if (lastNumber > dayRecordNumber)
            {
                descriptionValue.text = "рекорд дня";
            }
            else
            {
                descriptionValue.text = "рекорд " + recordNumber;
            }

            UpdateMedalIcon(lastNumber);
        }

        private void UpdateMedalIcon(int lastNumber)
        {
            medals.ForEach(data => data.SetActive(false));
        
            for (var i = _staticDataService.Progress.medalsValue.Count; i >= 0; i--)
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
