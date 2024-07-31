using System.Collections.Generic;
using UnityEngine;

namespace Fingers.Gameplay.Enemies
{
    public class EnemiesGroup : MonoBehaviour
    {
        [SerializeField] private Transform topBorderPoint;
        [SerializeField] private List<Enemy> enemies;

        private bool _isReady;
        private float _height;

        public Transform TopBorderPoint => topBorderPoint;

        public bool IsReady
        {
            get => _isReady;
            set => _isReady = value;
        }

        public float Height => _height;

        private void Start()
        {
            _height = Vector3.Distance(transform.position, topBorderPoint.position);
        }

        public void Activate()
        {
            enemies.ForEach(data => data.Activate());
        }

        public void Deactivate()
        {
            enemies.ForEach(data => data.Deactivate());
            _isReady = false;
        }

        public void Movement(float speedMove)
        {
            transform.Translate(0f, speedMove, 0f);
        }
    }
}