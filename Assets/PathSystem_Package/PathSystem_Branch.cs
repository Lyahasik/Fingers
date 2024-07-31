using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PathSystem
{
    /// <summary>
    /// PathSystem_Branch - general script for 'path branch'. Here the nodes are stored and used for path generation.
    /// </summary>
    [AddComponentMenu("Matej Vanco/Path System/Branch")]
    public class PathSystem_Branch : MonoBehaviour
    {
        public bool psAdjustNodesRotation = true;

        public bool psSetToBezier = false;
        [Range(6,80)]
        public int psBezierQuality = 32;

        public enum GetNodesOption { GetAllChildren, GetAllChildrenByScript, GetAllChildrenByTag, SpecificObjects };
        public GetNodesOption psGetNodesOption = GetNodesOption.GetAllChildren;

        public Transform psRootNode;
        //---By Component
        public MonoBehaviour psGetByScript;
        //---By Tag
        public string psGetByTag;

        public List<Transform> psNodes;

        public bool psGenerateLineRenderer = false;
        public bool psCustomLineRenderer = false;
        public bool psUpdateLineRendererEveryFrame = false;
        public bool psLoopLine = true;
        public LineRenderer psCustomLineRendererObj;
        public float psLineRendererWidth = 0.5f;
        public Color psLineRendererColor = Color.white;

        public bool psEnableGizmos = true;
        public Color psPathColor = Color.green;
        public Color psLabelColor = Color.cyan;
        public int psLabelSize = 12;
        public float psLabelOffsetSize = 1;
        public float psPathHandlesSize = 0.5f;

        public PathSystem_BezierSource psBezierBase;

        private LineRenderer createdLineR;

        private void Awake()
        {
            if (psRootNode == null)
                psRootNode = transform;
            pathSys_RefreshNodes(psGetNodesOption);

            if(psGenerateLineRenderer)
            {
                LineRenderer l;
                if (psCustomLineRenderer)
                    l = Instantiate(psCustomLineRendererObj);
                else
                {
                    l = new GameObject("BranchLine_" + this.name).AddComponent<LineRenderer>();
                    Material m = new Material(Shader.Find("Diffuse")){color = psLineRendererColor};
                    l.material = m;
                    l.widthMultiplier = psLineRendererWidth;
                    l.startColor = psLineRendererColor;
                    l.endColor = psLineRendererColor;
                }
                createdLineR = l;
                pathSys_RefreshLineRenderer();
            }
        }

        private void Update()
        {
            if (!psUpdateLineRendererEveryFrame)
                return;
            pathSys_RefreshLineRenderer();
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (UnityEditor.Selection.activeGameObject == this.gameObject)
                return;
            if (!psEnableGizmos)
                return;
            if (transform.childCount < 1)
                return;

            if (psBezierBase == null && psSetToBezier)
                psBezierBase = new PathSystem_BezierSource();

            Gizmos.color = psPathColor;

            for (int i = 0; i < transform.childCount; i++)
            {
                Transform node0 = transform.GetChild(i);

                if (psSetToBezier)
                {
                    Transform node1 = null;
                    if (i + 1 < transform.childCount)
                        node1 = transform.GetChild(i + 1);
                    else
                        node1 = transform.GetChild(0);

                    PathSystem_BezierSource.BezierParams_ bp = new PathSystem_BezierSource.BezierParams_();
                    bp.p1 = node0;
                    bp.p2 = node1;
                    bp.b = node0.GetChild(0);

                    for (int x = 0; x < psBezierBase.BezierQuality; x++)
                    {
                        Vector3 vec1 = psBezierBase.GetPoint(bp, x);
                        Vector3 vec2 = psBezierBase.GetPoint(bp, x + 1);
                        Gizmos.DrawLine(vec1, vec2);
                    }
                }
                else if (i + 1 < transform.childCount)
                    Gizmos.DrawLine(node0.position, transform.GetChild(i + 1).position);
                else
                    Gizmos.DrawLine(node0.position, transform.GetChild(0).position);
            }
        }
#endif

        /// <summary>
        /// Refresh all nodes by the specific GetNodesOption
        /// </summary>
        public void pathSys_RefreshNodes(GetNodesOption Option)
        {
            if(psGetNodesOption != GetNodesOption.SpecificObjects)
                psNodes.Clear();

            if (psGetNodesOption != GetNodesOption.SpecificObjects && psRootNode == null)
            {
                Debug.LogError("Path System_Branch: Node Root is missing");
                this.enabled = false;
                return;
            }

            switch (psGetNodesOption)
            {
                case GetNodesOption.GetAllChildren:
                    foreach (Transform child in psRootNode.GetComponentsInChildren<Transform>())
                    {
                        if (child.parent != psRootNode)
                            continue;
                        psNodes.Add(child);
                    }
                    break;
                case GetNodesOption.GetAllChildrenByTag:
                    foreach (Transform child in psRootNode.GetComponentsInChildren<Transform>())
                    {
                        if (child.parent != psRootNode)
                            continue;
                        if (child.tag == psGetByTag)
                            psNodes.Add(child);
                    }
                    break;
                case GetNodesOption.GetAllChildrenByScript:
                    foreach (Transform child in psRootNode.GetComponentsInChildren<Transform>())
                    {
                        if (child.parent != psRootNode)
                            continue;
                        if (child.GetComponent(psGetByScript.ToString()))
                            psNodes.Add(child);
                    }
                    break;
                case GetNodesOption.SpecificObjects:
                    if (psNodes.Count == 0)
                        Debug.Log("Path System_Branch: You are missing branch nodes... The script continues");
                    break;
            }

            if (psNodes.Contains(psRootNode))
                psNodes.Remove(psRootNode);

            if (psSetToBezier)
            {
                if (psBezierBase == null)
                    psBezierBase = new PathSystem_BezierSource();
                psBezierBase.BezierQuality = psBezierQuality;
            }

            if (psAdjustNodesRotation == false)
                return;

            for (int i = 1; i < psNodes.Count; i++)
            {
                Transform child = null;
                if (psSetToBezier)
                {
                    child = psNodes[i].GetChild(0);
                    child.parent = null;
                }
                psNodes[i].rotation = Quaternion.LookRotation(psNodes[i - 1].position - psNodes[i].position);
                if (psSetToBezier)
                    child.parent = psNodes[i];
            }
        }

        /// <summary>
        /// Refresh all nodes in specific node root
        /// </summary>
        public void pathSys_RefreshNodes(Transform NodeRoot)
        {
            if (NodeRoot == null)
                psRootNode = this.transform;
            else
                psRootNode = NodeRoot;
            pathSys_RefreshNodes(psGetNodesOption);
        }

        /// <summary>
        /// Refresh line renderer (if possible & enabled)
        /// </summary>
        public void pathSys_RefreshLineRenderer()
        {
            if (!createdLineR)
                return;
            LineRenderer l = createdLineR;
            l.positionCount = (psSetToBezier) ? (psLoopLine ? (psNodes.Count * psBezierQuality) + psBezierQuality : (psNodes.Count * psBezierQuality)) : psNodes.Count;
            if (psSetToBezier == false) //----------NonBezier line
            {
                for (int i = 0; i < psNodes.Count; i++)
                {
                    Transform ch = psNodes[i];
                    l.SetPosition(i, ch.position);
                }
                if (psLoopLine)
                {
                    l.positionCount += 2;
                    l.SetPosition(l.positionCount - 2, psNodes[psNodes.Count - 1].position);
                    l.SetPosition(l.positionCount - 1, psNodes[0].position);
                }
            }
            else //----------Bezier line
            {
                int x = 0;
                for (int i = 0; i < psNodes.Count; i++)
                {
                    l.SetPosition(x, psNodes[i].position);
                    bool returnBack = false;

                    while (!returnBack)
                    {
                        x++;
                        PathSystem_BezierSource.BezierParams_ bParam = new PathSystem_BezierSource.BezierParams_();
                        bParam.p1 = psNodes[i];
                        if (i + 1 < psNodes.Count)
                            bParam.p2 = psNodes[i + 1];
                        else if (psLoopLine)
                            bParam.p2 = psNodes[0];
                        else
                            break;
                        bParam.b = psNodes[i].GetChild(0);
                        Vector3 ppp = psBezierBase.GetPoint(bParam, ref returnBack);
                        l.SetPosition(x, ppp);
                        psBezierBase.NextIndex();
                    }
                }
                for (int i = l.positionCount - 1; i >= 0; i--)
                {
                    if (l.GetPosition(i) == Vector3.zero)
                        l.positionCount--;
                }
            }
        }
    }
}
