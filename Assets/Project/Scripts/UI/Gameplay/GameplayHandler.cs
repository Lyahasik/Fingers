using UnityEngine;

namespace Fingers.UI.Gameplay
{
    public class GameplayHandler : MonoBehaviour
    {
        private void Awake()
        {
            GetComponent<Canvas>().worldCamera = Camera.main;
        }
    }
}
