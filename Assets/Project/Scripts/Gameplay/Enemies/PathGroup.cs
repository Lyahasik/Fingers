using System.Collections.Generic;
using UnityEngine;

namespace Fingers.Gameplay.Enemies
{
    public class PathGroup : MonoBehaviour
    {
        [SerializeField] private List<PathAnimate> paths;

        public void Activate()
        {
            paths.ForEach(data => data.Activate());
        }

        public void Deactivate()
        {
            paths.ForEach(data => data.Deactivate());
        }
    }
}