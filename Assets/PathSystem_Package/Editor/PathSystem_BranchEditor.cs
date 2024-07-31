using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PathSystem
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PathSystem_Branch))]
    public class PathSystem_BranchEditor : Editor
    {
        private PathSystem_Branch targetobj;

        private void OnEnable()
        {
            targetobj = (PathSystem_Branch)target;
        }

        private void CreatePoint(int insert = -1)
        {
            GameObject newNode = new GameObject("Point" + (targetobj.transform.childCount+1));
            newNode.transform.parent = targetobj.transform;
            GameObject newNodeBezier = new GameObject("PointBezier" + (targetobj.transform.childCount+1));
            newNodeBezier.transform.parent = newNode.transform;
            newNodeBezier.transform.localPosition = Vector3.zero;

            if(insert!=-1)
            {
                newNode.transform.position = targetobj.transform.GetChild(insert).position + Vector3.forward * 4;
                newNode.transform.SetSiblingIndex(insert);
            }
            else
            {
                if (targetobj.transform.childCount > 0)
                    newNode.transform.position = targetobj.transform.GetChild(targetobj.transform.childCount - 1).position + Vector3.forward * targetobj.transform.childCount;
                else
                    newNode.transform.position = targetobj.transform.position + Vector3.forward * targetobj.transform.childCount;
            }
        }

        private void RemovePoint(int atIndex = -1)
        {
            if (atIndex != -1)
            {
                try
                {
                    DestroyImmediate(targetobj.transform.GetChild(atIndex).gameObject);
                }
                catch { }
            }
            else
            {
                if(targetobj.transform.childCount>0)
                    DestroyImmediate(targetobj.transform.GetChild(targetobj.transform.childCount-1).gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            s();

            bv();
            DrawProperty("psRootNode", "Root Node", "Leave this field empty if the root node is THIS object [Default]");
            ev();
            if (targetobj.psGetNodesOption == PathSystem_Branch.GetNodesOption.GetAllChildren)
            {
                bh();
                if (GUILayout.Button("Add New Point", GUILayout.MaxWidth(240)))
                    CreatePoint();
                if (GUILayout.Button("Remove Last Point", GUILayout.MaxWidth(240)))
                    RemovePoint();
                eh();
                s(5);
                if (targetobj.transform.childCount > 0)
                {
                    bv();
                    string sel = "";
                    if (selIndex >= 0 && selIndex < targetobj.transform.childCount)
                        sel = targetobj.transform.GetChild(selIndex).name;
                    if (!string.IsNullOrEmpty(sel))
                    {
                        l("Selected: " + sel);
                        bh();
                        if (GUILayout.Button("Insert New Point", GUILayout.MaxWidth(200)))
                            CreatePoint(selIndex);
                        if (GUILayout.Button("Remove Current Point", GUILayout.MaxWidth(200)))
                            RemovePoint(selIndex);
                        eh();
                    }
                    ev();
                }
                s(5);
                if (GUILayout.Button("Clear All"))
                {
                    if (!EditorUtility.DisplayDialog("Warning", "This will clear all your points & you won't be able to undo. Are you sure?", "Yes", "No"))
                        return;
                    for (int i = targetobj.transform.childCount - 1; i >= 0; i--)
                        DestroyImmediate(targetobj.transform.GetChild(i).gameObject);
                    Repaint();
                    return;
                }
            }
            s();

            bv();
            DrawProperty("psAdjustNodesRotation", "Adjust Nodes Rotation", "If enabled, the nodes will adjust to the correct rotation");
            s(5);
            DrawProperty("psSetToBezier", "Use Bezier Curve", "If enabled, the path nodes will set to bezier curve editor [Suggestion: do not change this value on your own - nodes have to contain at least one children to represent Quadratic Bezier Curve Tangen]");
            if (targetobj.psSetToBezier)
                DrawProperty("psBezierQuality", "Bezier Quality","Bezier Quality value [The bigger value is, the more performance it will cost. Default - 60]");
            ev();

            s();
            l("Get Nodes Option");
            DrawProperty("psGetNodesOption", "Option","Select the option of getting nodes on start");
            bv();
            switch(targetobj.psGetNodesOption)
            {
                case PathSystem_Branch.GetNodesOption.GetAllChildrenByTag:
                    l("Nodes will be get from the roots children by tag");
                    DrawProperty("psGetByTag", "Tag");
;                    break;
                case PathSystem_Branch.GetNodesOption.GetAllChildrenByScript:
                    l("Nodes will be get from the roots children by included monobehaviour");
                    DrawProperty("psGetByScript", "Mono Behaviour");
                    break;
                case PathSystem_Branch.GetNodesOption.SpecificObjects:
                    l("Select specific nodes");
                    DrawProperty("psNodes", "Specific Objects","",true);
                    break;

                default:
                    l("Nodes will be get from the roots children");
                    l("Current children count: " + targetobj.transform.childCount.ToString());
                    break;
            }
            ev();

            bv();
            DrawProperty("psGenerateLineRenderer", "Generate Line Renderer");
            if (targetobj.psGenerateLineRenderer)
            {
                s(5);
                DrawProperty("psUpdateLineRendererEveryFrame", "Update Line Every Frame");
                DrawProperty("psCustomLineRenderer", "Get Line From Prefab");
                if(targetobj.psCustomLineRenderer)
                    DrawProperty("psCustomLineRendererObj", "Line Prefab");
                else
                {
                    DrawProperty("psLineRendererWidth", "Line Width");
                    DrawProperty("psLineRendererColor", "Line Color");
                }
                DrawProperty("psLoopLine", "Loop Line");
            }
            ev();

            bv();
            DrawProperty("psEnableGizmos", "Enable Gizmos");
            if (targetobj.psEnableGizmos)
            {
                s(5);
                DrawProperty("psPathColor", "Path Color");
                DrawProperty("psLabelColor", "Label Color");
                s(5);
                DrawProperty("psLabelSize", "Label Size");
                DrawProperty("psLabelOffsetSize", "Label Offset");
                DrawProperty("psPathHandlesSize", "Path Handles Size");
            }
            ev();
        }

        private void OnSceneGUI()
        {
            if (!targetobj.psEnableGizmos)
                return;
            if (targetobj.transform.childCount < 1)
                return;

            if (targetobj.psBezierBase == null && targetobj.psSetToBezier)
                targetobj.psBezierBase = new PathSystem_BezierSource();

            Handles.color = targetobj.psPathColor;

            for (int i = 0; i < targetobj.transform.childCount; i++)
            {
                Transform node0 = targetobj.transform.GetChild(i);
                node0.position = ReturnHandlePoint(node0.position, i);

                if (targetobj.psSetToBezier)
                {
                    Transform node1 = null;
                    if (i + 1 < targetobj.transform.childCount)
                        node1 = targetobj.transform.GetChild(i + 1);
                    else
                        node1 = targetobj.transform.GetChild(0);

                    PathSystem_BezierSource.BezierParams_ bp = new PathSystem_BezierSource.BezierParams_();
                    bp.p1 = node0;
                    bp.p2 = node1;
                    bp.b = node0.GetChild(0);

                    bp.b.position = ReturnHandlePoint(bp.b.position, i, false);

                    targetobj.psBezierBase.BezierQuality = targetobj.psBezierQuality;
                    targetobj.psBezierBase.BezierQuality = (targetobj.psBezierBase.BezierQuality <= 0) ? 1 : targetobj.psBezierBase.BezierQuality;
                    for (int x = 0; x < targetobj.psBezierBase.BezierQuality; x++)
                    {
                        Vector3 vec1 = targetobj.psBezierBase.GetPoint(bp, x);
                        Vector3 vec2 = targetobj.psBezierBase.GetPoint(bp, x + 1);
                        Handles.DrawLine(vec1, vec2);
                    }

                    Handles.DrawLine(node0.position, bp.b.position);
                }
                else if (i + 1 < targetobj.transform.childCount)
                    Handles.DrawLine(node0.position, targetobj.transform.GetChild(i + 1).position);
                else
                    Handles.DrawLine(node0.position, targetobj.transform.GetChild(0).position);
            }
        }

        private Quaternion hRot;
        private int selIndex;
        private Vector3 ReturnHandlePoint(Vector3 point, int indx, bool isRootPoint = true)
        {
            hRot = Tools.pivotRotation == PivotRotation.Local ? targetobj.transform.rotation : Quaternion.identity;
            float size = targetobj.psPathHandlesSize;
            if (isRootPoint == false)
                size /= 2;

            GUIStyle style = new GUIStyle();
            style.normal.textColor = targetobj.psLabelColor;
            style.fontSize = targetobj.psLabelSize;

            if (Handles.Button(point, hRot, size, size, Handles.DotHandleCap))
            {
                selIndex = indx;
                Repaint();
            }

            if (selIndex == indx)
            {
                EditorGUI.BeginChangeCheck();
                point = Handles.DoPositionHandle(point, hRot);
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(targetobj, "Point Moved");
                    EditorUtility.SetDirty(targetobj);
                }
            }

            if (isRootPoint)
                Handles.Label(point + Vector3.up * targetobj.psLabelOffsetSize + Vector3.right * targetobj.psLabelOffsetSize, indx.ToString(), style);

            return point;
        }

        private void bv()
        {
            EditorGUI.indentLevel += 1;
            GUILayout.BeginVertical("Box");
        }
        private void ev()
        {
            GUILayout.EndVertical();
            EditorGUI.indentLevel -= 1;
        }

        private void bh()
        {
            GUILayout.BeginHorizontal("Box");
            GUILayout.FlexibleSpace();
        }
        private void eh()
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        private void s(float siz = 10)
        {
            GUILayout.Space(siz);
        }
        private void l(string t)
        {
            GUILayout.Label(t);
        }
        private void DrawProperty(string propName, string propText = "", string toolTip = "", bool _list = false)
        {
            if (string.IsNullOrEmpty(propText))
                propText = propName;
            SerializedProperty s = serializedObject.FindProperty(propName);
            EditorGUILayout.PropertyField(s, new GUIContent(propText, toolTip), _list);
            serializedObject.ApplyModifiedProperties();
        }
    }
}