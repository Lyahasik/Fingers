using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PathSystem
{
    public class PathSystem_Editor : EditorWindow
    {
        public static string version = "Path System 1.0";
        public static PathSystem_Editor win;

        private static Texture2D iconDrawFree;
        private static Texture2D iconDrawLines;

        private static Transform drawGraphic;

        public static void pseAddToEditor()
        {
            SceneView.duringSceneGui += OnScene;
        }
        public static void pseRemoveFromEditor()
        {
            SceneView.duringSceneGui -= OnScene;
        }

        [MenuItem("Window/Path Creator")]
        public static void pseInitialize()
        {
            PathSystem_Editor pth = (PathSystem_Editor)GetWindow(typeof(PathSystem_Editor));
            pth.minSize = new Vector2(350, 300);
            pth.maxSize = new Vector2(350, 300);
            win = pth;
            pth.Show();

            iconDrawFree = (Texture2D)Resources.Load("iconFreeDraw", typeof(Texture2D));
            iconDrawLines = (Texture2D)Resources.Load("iconLineDraw", typeof(Texture2D));

            pseMinimumDistancestr = pseMinimumDistance.ToString();

            if(drawGraphic != null)
                DestroyImmediate(drawGraphic.gameObject);

            drawGraphic = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
            Material graphicMaterial = new Material(Shader.Find("Unlit/Color"));
            graphicMaterial.SetColor("_Color", Color.green);
            drawGraphic.GetComponent<Renderer>().sharedMaterial = graphicMaterial;
            DestroyImmediate(drawGraphic.GetComponent<Collider>());
            drawGraphic.name = "InternalPathDrawingGraphic";
            drawGraphic.hideFlags = HideFlags.HideInHierarchy;

            SceneView.duringSceneGui += OnScene;
        }

        [MenuItem("GameObject/Create Other/Path System/Create Line")]
        public static void pseCreateLine()
        {
            GameObject newLine = new GameObject("PathBranch_Line");
            newLine.AddComponent<PathSystem_Branch>();
            newLine.transform.position = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 8f;

            GameObject n = new GameObject("Point0");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero;
            GameObject n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + Vector3.forward * 6f + Vector3.left * 6f;
            n = new GameObject("Point1");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero + Vector3.forward * 12f;
            n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + -Vector3.forward * 6f + Vector3.right * 6f;
        }

        [MenuItem("GameObject/Create Other/Path System/Create Triangle")]
        public static void pseCreateTriangle()
        {
            GameObject newLine = new GameObject("PathBranch_Triangle");
            newLine.AddComponent<PathSystem_Branch>();
            newLine.transform.position = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 8f;

            GameObject n = new GameObject("Point0");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero;
            GameObject n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + Vector3.right * 12f;
            n = new GameObject("Point1");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero + Vector3.forward * 12f + Vector3.right * 12f;
            n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + -Vector3.right * 12f + Vector3.forward * 6f;
            n = new GameObject("Point2");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero + Vector3.forward * 12f + -Vector3.right * 12f;
            n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + -Vector3.forward * 12f;
        }

        [MenuItem("GameObject/Create Other/Path System/Create Square")]
        public static void pseCreateSquare()
        {
            GameObject newLine = new GameObject("PathBranch_Square");
            newLine.AddComponent<PathSystem_Branch>();
            newLine.transform.position = SceneView.lastActiveSceneView.camera.transform.position + SceneView.lastActiveSceneView.camera.transform.forward * 8f;

            GameObject n = new GameObject("Point0");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero;
            GameObject n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + Vector3.right * 6f + -Vector3.forward * 6f;
            n = new GameObject("Point1");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero + Vector3.right * 12f;
            n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + Vector3.right * 6f + Vector3.forward * 6f;
            n = new GameObject("Point2");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero + Vector3.forward * 12f + Vector3.right * 12f;
            n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + Vector3.forward * 6f + -Vector3.right * 6;
            n = new GameObject("Point3");
            n.transform.parent = newLine.transform;
            n.transform.localPosition = Vector3.zero + Vector3.forward * 12f;
            n1 = new GameObject("Bezier");
            n1.transform.parent = n.transform;
            n1.transform.localPosition = Vector3.zero + -Vector3.right * 6f + -Vector3.forward * 6f;
        }

        void OnDestroy()
        {
            DestroyImmediate(drawGraphic.gameObject);
            drawGraphic = null;
            SceneView.duringSceneGui -= OnScene;
        }

        void OnGUI()
        {
            s();
            l(version, 18, TextAnchor.MiddleCenter);
            p();
            l("Drawing tool");
            bh();
            if(b("", iconDrawFree))
                drawType = _DrawType.Free;
            s(5);
            if (b("", iconDrawLines))
                drawType = _DrawType.Lines;
            eh();
            l(drawType.ToString(),9, TextAnchor.MiddleCenter);
            p();
            if (drawType == _DrawType.Free)
            {
                l("Minimum Drawing Distance");
                pseMinimumDistancestr = GUILayout.TextField(pseMinimumDistancestr);
                if (b("Apply Distance Value", null, false))
                {
                    float.TryParse(pseMinimumDistancestr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out pseMinimumDistance);
                    if (pseMinimumDistance < 1.0f)
                    {
                        EditorUtility.DisplayDialog("Warning", "Minimum distance value is 1.0!", "OK");
                        pseMinimumDistance = 1.0f;
                    }
                }
            }
            p();
            pseCurrentBranch = EditorGUILayout.ObjectField("Draw Into Branch", pseCurrentBranch, typeof(PathSystem_Branch), true) as PathSystem_Branch;
            if (drawType == _DrawType.Free)
                pseCreateNewBranchOnRelease = GUILayout.Toggle(pseCreateNewBranchOnRelease, "Create New Branch On Release");
            else
            {
                if(b("Create New Path Branch",null,false))
                {
                    previousPoint = null;
                    currentPoint = null;
                    pseCurrentBranch = null;
                    keyHold = false;
                    pressed = false;
                }
            }
            p();
        }

        #region GUI Internal & Editor Methods
        private static bool b(string t, Texture2D i, bool Rectangular = true)
        {
            if(Rectangular)
                return GUILayout.Button(new GUIContent(t, i), GUILayout.Width(60), GUILayout.Height(60));
            else
                return GUILayout.Button(new GUIContent(t, i));
        }
        private static void bv()
        {
            GUILayout.BeginVertical("Box");
        }
        private static void ev()
        {
            GUILayout.EndVertical();
        }
        private static void bh()
        {
            GUILayout.BeginHorizontal("Box");
            GUILayout.FlexibleSpace();
        }
        private static void eh()
        {
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        private static void s(float siz = 10)
        {
            GUILayout.Space(siz);
        }
        private static void l(string t, int fontSize = 10, TextAnchor align = TextAnchor.MiddleLeft)
        {
            GUIStyle style = new GUIStyle();
            style.normal.textColor = Color.white;
            style.fontSize = fontSize;
            style.alignment = align;
            try
            {
                GUILayout.Label("   " + t, style);
            }
            catch { }
        }
        private static void p()
        {
            s(5);
            GUILayout.Box("", GUILayout.Height(5),GUILayout.Width(win.position.width));
            s(5);
        }
        #endregion

        public enum _DrawType { Free, Lines};
        public static _DrawType drawType;

        public static PathSystem_Branch pseCurrentBranch;
        public static float pseMinimumDistance = 5;
        private static string pseMinimumDistancestr;
        private static bool pseCreateNewBranchOnRelease = true;

        private static Transform previousPoint;
        private static Transform currentPoint;

        private static bool keyHold = false;
        private static bool pressed = false;

        private static void OnScene(SceneView sceneview)
        {
            Handles.BeginGUI();
            GUILayout.BeginArea(new Rect(20, 10, SceneView.lastActiveSceneView.position.width-40, SceneView.lastActiveSceneView.position.height-30));

            bh();
            if (drawType == _DrawType.Free)
            {
                GUILayout.Box(iconDrawFree, GUILayout.Width(30), GUILayout.Height(30));
                bv();
                l("Locate cursor on object with collider, hold Left-Control to draw and move with the cursor");
                if(pseCurrentBranch)
                    l("Branch Points: "+pseCurrentBranch.transform.childCount.ToString());
                ev();
            }
            else
            { 
                GUILayout.Box(iconDrawLines, GUILayout.Width(30), GUILayout.Height(30));
                bv();
                l("Locate cursor on object with collider & press Left-Control to create line");
                if (pseCurrentBranch)
                    l("Branch Points: " + pseCurrentBranch.transform.childCount.ToString());
                ev();
            }
            eh();

            GUILayout.EndArea();
            Handles.EndGUI();

            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            RaycastHit hit;
            bool raying = Physics.Raycast(ray, out hit);
            bool hitcol = hit.collider;

            if(raying && hitcol)
                drawGraphic.position = hit.point;

            if (!Event.current.control && keyHold)
            {
                previousPoint = null;
                currentPoint = null;
                if (pseCreateNewBranchOnRelease && drawType != _DrawType.Lines)
                    pseCurrentBranch = null;
                keyHold = false;
                pressed = false;
                return;
            }
            else if (Event.current.control && !keyHold)
                keyHold = true;
            else if (!Event.current.control)
                return;

            if (!raying || !hitcol)
                return;

            if (pseCurrentBranch == null)
            {
                pseCurrentBranch = new GameObject("PathBranch").AddComponent<PathSystem_Branch>();
                Transform newPoint = new GameObject("Point0").transform;
                Transform newPoint2 = new GameObject("PointBezier" + pseCurrentBranch.transform.childCount.ToString()).transform;
                newPoint2.parent = newPoint;
                newPoint2.localPosition = Vector3.zero;
                newPoint.position = hit.point;
                previousPoint = newPoint;
                previousPoint.parent = pseCurrentBranch.transform;
            }
            else
            {
                if (drawType == _DrawType.Free)
                {
                    if (previousPoint == null && pseCurrentBranch.transform.childCount > 0)
                        previousPoint = pseCurrentBranch.transform.GetChild(pseCurrentBranch.transform.childCount - 1);
                    if (currentPoint != null)
                    {
                        currentPoint.position = hit.point;
                        if (Vector3.Distance(previousPoint.position, currentPoint.position) > pseMinimumDistance)
                        {
                            previousPoint = currentPoint;
                            Transform newPoint = new GameObject("Point" + pseCurrentBranch.transform.childCount.ToString()).transform;
                            Transform newPoint2 = new GameObject("PointBezier" + pseCurrentBranch.transform.childCount.ToString()).transform;
                            newPoint2.parent = newPoint;
                            newPoint2.localPosition = Vector3.zero;
                            currentPoint = newPoint;
                            currentPoint.parent = pseCurrentBranch.transform;
                        }
                    }
                    else
                    {
                        Transform newPoint = new GameObject("Point" + pseCurrentBranch.transform.childCount.ToString()).transform;
                        Transform newPoint2 = new GameObject("PointBezier" + pseCurrentBranch.transform.childCount.ToString()).transform;
                        newPoint2.parent = newPoint;
                        newPoint2.localPosition = Vector3.zero;
                        newPoint.position = hit.point;
                        currentPoint = newPoint;
                        currentPoint.parent = pseCurrentBranch.transform;
                    }
                }
                else
                {
                    if (previousPoint == null && pseCurrentBranch.transform.childCount > 0)
                        previousPoint = pseCurrentBranch.transform.GetChild(pseCurrentBranch.transform.childCount - 1);

                    if (pressed)
                        return;
                   
                    Transform newPoint = new GameObject("Point" + pseCurrentBranch.transform.childCount.ToString()).transform;
                    Transform newPoint2 = new GameObject("PointBezier" + pseCurrentBranch.transform.childCount.ToString()).transform;
                    newPoint2.parent = newPoint;
                    newPoint2.localPosition = Vector3.zero;
                    newPoint.position = hit.point;
                    currentPoint = newPoint;
                    currentPoint.parent = pseCurrentBranch.transform;
                    pressed = true;
                }
            }
        }
    }
}
