using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Fingers.UI.Localization
{
    public class LocaleItem : Toggle
    {
        [SerializeField] private UnityEvent onSelect;

        public override void OnPointerClick(PointerEventData eventData)
        {
            base.OnPointerClick(eventData);
            
            onSelect?.Invoke();
        }
    }
}
