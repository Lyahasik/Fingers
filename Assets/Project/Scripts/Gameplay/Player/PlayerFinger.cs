using UnityEngine;

namespace Fingers.Gameplay.Player
{
    public class PlayerFinger : MonoBehaviour
    {
        [SerializeField] private GameObject trail;
        
        private Vector3 _startPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            
            trail.SetActive(false);
        }

        public void UpdatePosition(in Vector3 newPosition)
        {
            transform.localPosition = new Vector3(newPosition.x, newPosition.y, _startPosition.z);
            
            if (!trail.activeSelf)
                trail.SetActive(true);
        }

        public void ResetPosition()
        {
            transform.position = _startPosition;
            trail.SetActive(false);
        }
    }
}