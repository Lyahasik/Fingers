using UnityEngine;

namespace Fingers.Gameplay.Player
{
    public class PlayerFinger : MonoBehaviour
    {
        [SerializeField] private GameObject trail;
        
        private Vector3 _startPosition;
        private Vector3 _position;

        public Vector3 Position => _position;

        private void Awake()
        {
            _startPosition = transform.position;
            
            trail.SetActive(false);
        }

        public void UpdatePosition(in Vector3 newPosition)
        {
            _position = newPosition;
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