using System.Collections;
using Fingers.Core.Coroutines;
using Fingers.UI.StaticData;
using UnityEngine;

namespace Fingers.UI.Loading
{
    public class LoadingCurtain : MonoBehaviour
    {
        [SerializeField] private CanvasGroup curtain;
        
        private UIStaticData _uiStaticData;

        private WaitForSeconds _dissolutionDelay;

        public void Construct(string newName, UIStaticData uiStaticData)
        {
            name = newName;
            _uiStaticData = uiStaticData;
        }

        private void Awake()
        {
            DontDestroyOnLoad(this);
            
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Show()
        {
            gameObject.SetActive(true);
            curtain.alpha = 1;
        }

        public void Hide(ICoroutineRunnerService container) =>
            container.StartCoroutine(DoFadeIn());

        private IEnumerator DoFadeIn()
        {
            _dissolutionDelay ??= new WaitForSeconds(_uiStaticData.curtainDissolutionDelay);
            
            while (curtain.alpha > 0)
            {
                curtain.alpha -= _uiStaticData.curtainDissolutionStep;
                yield return _dissolutionDelay;
            }
      
            gameObject.SetActive(false);
        }
    }
}