using UnityEngine;

namespace Fingers.Gameplay.Player
{
    public class PlayerFinger : MonoBehaviour
    {
        private Vector3 _startPosition;
        private Vector2 _shiftPosition;

        private void Awake()
        {
            _startPosition = transform.position;
            
            var parentRectTransform = transform.parent.GetComponent<RectTransform>();
            _shiftPosition = new Vector2(parentRectTransform.rect.width * 0.5f, parentRectTransform.rect.height * 0.5f);
        }

        public void UpdatePosition(in Vector3 newPosition)
        {
            transform.localPosition = newPosition - (Vector3) _shiftPosition;
        }

        public void ResetPosition()
        {
            transform.position = _startPosition;
        }
    }
}