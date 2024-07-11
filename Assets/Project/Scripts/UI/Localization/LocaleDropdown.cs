using Fingers.Core.Progress;
using Fingers.Core.Services.Localization;
using Fingers.Core.Services.Progress;
using TMPro;
using UnityEngine.Localization.Settings;

namespace Fingers.UI.Localization
{
    public class LocaleDropdown : TMP_Dropdown, IWritingProgress
    {
        private IProgressProviderService _progressProviderService;
        private ILocalizationService _localizationService;

        private int _localeId;

        public void Construct(ILocalizationService localizationService,
            IProgressProviderService progressProviderService)
        {
            _localizationService = localizationService;
            _progressProviderService = progressProviderService;
        }

        public void Initialize()
        {
            onValueChanged.AddListener(LocaleSelected);
            
            Register(_progressProviderService);
        }

        public void Register(IProgressProviderService progressProviderService)
        {
            progressProviderService.Register(this);
        }

        public void LoadProgress(ProgressData progress)
        {
            _localeId = progress.LocaleId;
            UpdateValues();
        }

        public void UpdateProgress(ProgressData progress) {}

        public void WriteProgress()
        {
            _progressProviderService.SetLocale(_localeId);
        }

        private void UpdateValues()
        {
            value = _localeId;
            
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[value];

            interactable = true;
        }

        private void LocaleSelected(int index)
        {
            _localeId = index;
            _localizationService.UpdateLocale(_localeId);
            
            WriteProgress();
        }
    }
}