using System;
using System.Collections;
using Fingers.Constants;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace Fingers.Core.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        public event Action OnUpdateLocale;

        public IEnumerator Initialize()
        {
            yield return LocalizationSettings.InitializationOperation;
            
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void UpdateLocale(int localeId)
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localeId];
            
            OnUpdateLocale?.Invoke();
        }

        public string LocaleMain(string keyValue, TMP_Text textObject) => 
            LocaleResourceAsync(ConstantValues.LOCALE_MAIN_TABLE, keyValue, textObject);

        private string LocaleResourceAsync(string tableName, string keyValue, TMP_Text textObject = null)
        {
            if (textObject != null)
            {
                var op = LocalizationSettings
                    .StringDatabase
                    .GetLocalizedStringAsync(tableName, keyValue);
                if (op.IsDone)
                    textObject.text = op.Result;
                else
                    op.Completed += data => textObject.text = data.Result;
            }
            else
            {
                return LocalizationSettings.StringDatabase.GetLocalizedString(tableName, keyValue);
            }
            
            return null;
        }
    }
}