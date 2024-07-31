using UnityEngine;
using UnityEngine.Splines;

namespace Fingers.Gameplay.Enemies
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private SplineAnimate splineAnimate;

        public void Activate()
        {
            splineAnimate.Play();
        }

        public void Deactivate()
        {
            splineAnimate.Pause();
        }
    }
}
