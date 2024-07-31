using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
    /// <summary>
    /// Path System - main component for bezier curves on pathes.
    /// </summary>
    public class PathSystem_BezierSource
    {
        public int BezierQuality = 32; //---Quality can be changed anytime, 32 is default.
        private int BezierIndex = 0;

        public struct BezierParams_
        {
            public Transform p1;
            public Transform p2;
            public Transform b;
        }

        /// <summary>
        /// Get point from bezier branch
        /// </summary>
        public Vector3 GetPoint(BezierParams_ BezierParams, ref bool pointBranchisDone)
        {
            float t = BezierIndex / (float)BezierQuality;
            if (BezierIndex > BezierQuality)
            {
                pointBranchisDone = true;
                BezierIndex = 0;
            }
            return BezierFormula(BezierParams.p1.position, BezierParams.b.position, BezierParams.p2.position, t);
        }
        /// <summary>
        /// Get point from bezier branch
        /// </summary>
        public Vector3 GetPoint(BezierParams_ BezierParams, int index)
        {
            float t = index / (float)BezierQuality;
            return BezierFormula(BezierParams.p1.position, BezierParams.b.position, BezierParams.p2.position, t);
        }
        public void NextIndex()
        {
            BezierIndex++;
        }

        private static Vector3 BezierFormula(Vector3 p1, Vector3 b, Vector3 p2, float t)
        {
            float u = 1 - t;
            float tt = t * t;
            float uu = u * u;
            Vector3 p = uu * p1;
            p += 2 * u * t * b;
            p += tt * p2;
            return p;
        }
    }
}