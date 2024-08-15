using TMPro;
using UnityEngine;

namespace Fingers
{
    public class MedalProgress : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        public void Activate(int number)
        {
            gameObject.SetActive(true);
            text.text = number.ToString();
        }
    }
}
