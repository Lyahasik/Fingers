using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace PathSystem
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(PathSystem_Object))]
    public class PathSystem_ObjectEditor : Editor
    {
        
        public override void OnInspectorGUI()
        {
            PathSystem_Object targetobj = (PathSystem_Object)target;

            s();

            bv();
            s(5);
            DrawProperty("psSimulateOnStart", "Simulate On Start", "If enabled, the object will start following the path branch");
            DrawProperty("psUpdateMovement", "Update Movement", "If enabled, the object will move along the path right after the start");
            DrawProperty("psContinueInPath", "Continue In Path", "If disabled, the object on start will be teleported to the first path node. Otherwise will smoothly continue from the current position");
            s(5);
            bv();
            DrawProperty("psCurrentBranch", "Target Path Branch");
            ev();
            s();
            if (!targetobj.psUpdateMovement)
                GUI.color = Color.gray;
            else
                GUI.color = Color.white;
            l("Essential Parameters");
            bv();
            DrawProperty("psRadialType", "• Radial Type", "If enabled, the node will be set to radial-path system");
            DrawProperty("psUseGravity", "• Use Gravity", "If enabled, the node will accelerate by it's angle & apply true laws of physics");
            DrawProperty("psCopyPathRotation", "• Copy Path Rotation", "If enabled, the object will rotate along the path direction");
            if (targetobj.psCopyPathRotation)
                DrawProperty("psUseNaturalAngularRotation", "   • Use Angular Rotation", "If enabled, the node will rotate by the impact of the upcoming angle");
            else
                DrawProperty("psLookAt", "• Look At", "If enabled, the object will be able to 'look at' the specific object");
            s(3);
            DrawProperty("psMinimumNodeDistance", "Min Node Distance", "Minimum node distance - the less number is, the more time it takes to travel to the node");
            ev();
            
            s();

            l("Object Additional Offsets");
            bv();
            if (targetobj.psRadialType)
            {
                bv();
                DrawProperty("psRadius", "Radius", "Positional-radius of the object on nodes");
                ev();
            }
            if(targetobj.psLookAt && !targetobj.psCopyPathRotation)
            {
                bv();
                DrawProperty("psLookAtSource", "Look At Target");
                ev();
            }
            DrawProperty("psPositionOffset", "Position Offset", "Object position offset on the path");
            DrawProperty("psRotationOffset", "Rotation Offset", "Object rotation offset on the path");
            ev();
            s();

            l("Value Transitions");
            bv();
            DrawProperty("psMovementSmooth", "Movement Smooth");
            DrawProperty("psRotationSmooth", "Rotation Smooth");
            s(3);
            DrawProperty("psSpeedTransition", "Speed Transition","The value of the speed transition");
            DrawProperty("psSpeedBreak", "Speed Break","Initial speed breaker");
            DrawProperty("psTurnTransition", "Turn Transition","The value of the turning transition");
            ev();

            s(5);

            l("Speed Parameters");
            bv();
            if(!targetobj.psUseGravity)
                DrawProperty("psDefaultSpeed", "Default Speed");
            s(2);
            DrawProperty("psMaxSpeed", "Max Speed");
            DrawProperty("psMinSpeed", "Min Speed");
            ev();

            s(5);

            if (targetobj.psCopyPathRotation && targetobj.psUseNaturalAngularRotation)
            {
                l("Turning Parameters");
                bv();
                DrawProperty("psTurnSwingBias", "Turn Swing Bias","Turning bias breaker");
                DrawProperty("psTurnSwingMaxAngle", "Max Turn Angle");
                ev();

                s(5);
            }

            l("Current Essential Parameters");
            bv();
            DrawProperty("psCurrentSpeed", "Current Speed");
            DrawProperty("psCurrentBoost", "Current Boost");
            DrawProperty("psCurrentTurn", "Current Turn");
            ev();

            s();

            l("Audio & Sounds");
            bv();
            DrawProperty("psUseAudioEffect", "Use Audio Effect");
            if(targetobj.psUseAudioEffect)
                DrawProperty("psAudioEffect", "Audio Source");
            ev();

            s();

            l("Path Options");
            bv();
            DrawProperty("psLoopPath", "Loop Path");
            DrawProperty("psAdaptToUpcomingNode", "Adapt To Upcoming Node", "If disabled, the object after the end of the path will reset it's position & rotation to the first node in branch");
            if (targetobj.psLoopPath == false)
                DrawProperty("psRounds", "Count Of Rounds");
            DrawProperty("psOnPathEnd", "On Path End");
            ev();

            s();

            ev();

            serializedObject.Update();
            serializedObject.ApplyModifiedProperties();
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
        private void s(float siz = 10)
        {
            GUILayout.Space(siz);
        }
        private void l(string t)
        {
            GUILayout.Label(t);
        }
        private void DrawProperty(string propName, string propText = "", string toolTip = "")
        {
            if (string.IsNullOrEmpty(propText))
                propText = propName;
            SerializedProperty s = serializedObject.FindProperty(propName);
            EditorGUILayout.PropertyField(s, new GUIContent(propText, toolTip));
            serializedObject.ApplyModifiedProperties();
        }
    }
}
