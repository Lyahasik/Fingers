using TMPro;
using UnityEngine;

namespace Fingers.UI.Hud
{
    public class HudView : MonoBehaviour
    {
        [SerializeField] private TMP_Text scoreText;
        
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }

        public void Initialize()
        {
            Debug.Log($"[{ GetType() }] initialize");
        }

        public void UpdateScores(int scores)
        {
            scoreText.text = scores.ToString();
        }
    }
}
