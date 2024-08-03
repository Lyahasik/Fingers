using UnityEngine;

namespace Fingers.Gameplay.Player
{
    public class PlayerFinger : MonoBehaviour
    {
        [SerializeField] private GameObject trail;
        
        private Vector3 _startPosition;
        private Vector2 _shiftPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            
            var parentRectTransform = transform.parent.GetComponent<RectTransform>();
            _shiftPosition = new Vector2(parentRectTransform.rect.width * 0.5f, parentRectTransform.rect.height * 0.5f);
            
            trail.SetActive(false);
        }

        public void UpdatePosition(in Vector3 newPosition)
        {
            transform.localPosition = newPosition - (Vector3) _shiftPosition;
            
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