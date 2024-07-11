using System;
using TMPro;

namespace Fingers.Core.Services.Localization
{
    public interface ILocalizationService : IService
    {
        public event Action OnUpdateLocale;
        public void UpdateLocale(int localeId);
        public string LocaleMain(string keyValue, TMP_Text textObject = null);
    }
}