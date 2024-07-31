using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
    /// <summary>
    /// Path System - additional system for nodes [to control node boost, manual turn value etc]
    /// </summary>
    [AddComponentMenu("Matej Vanco/Path System/Node")]
    public class PathSystem_Node : MonoBehaviour
    {
        [Space(10)]
        public float NodeBoost = 0;
        [Tooltip("Less number - Left Direction Turn, Higher number - Right Direction Turn")]
        [Range(-2,2)]
        public float NodeTurnValue = 0.0f;
        [Space]
        public UnityEngine.Events.UnityEvent NodeEvent;
        [Space]
        public bool enableGizmos = true;
        void OnDrawGizmos()
        {
            if (!enableGizmos)
                return;
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, transform.localScale.magnitude);
        }
    }
}
