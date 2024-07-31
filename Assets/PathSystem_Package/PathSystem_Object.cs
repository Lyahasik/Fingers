using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PathSystem
{
    /// <summary>
    /// Path System - main component for objects 'riding' on pathes.
    /// </summary>
    [AddComponentMenu("Matej Vanco/Path System/Object")]
    public class PathSystem_Object : MonoBehaviour
    {
        public bool psSimulateOnStart = true;
        public bool psUpdateMovement = true;
        public bool psContinueInPath = false;

        public PathSystem_Branch psCurrentBranch;

        public bool psLoopPath = true;
        public int psRounds = 1;
        public bool psAdaptToUpcomingNode = true;
        public UnityEngine.Events.UnityEvent psOnPathEnd;

        public Transform psCurrentNode;
        public Vector3 psCurrentNodePosition;

        public bool psUseGravity = false;
        public bool psRadialType = false;
        public bool psUseNaturalAngularRotation = true;
        public bool psCopyPathRotation = true;
        public bool psLookAt = false;
        public float psMinimumNodeDistance = 2.5f;

        public float psRadius = 1.7f;
        public Transform psLookAtSource;
        public Vector3 psPositionOffset = new Vector3(0, 0, 0);
        public Vector3 psRotationOffset = new Vector3(0, 0, 90);

        [Space]
        public float psMovementSmooth = 15.0f;
        public float psRotationSmooth = 9.0f;

        public float psSpeedTransition = 0.6f;
        public float psTurnTransition = 1.64f;

        [Space]
        public float psDefaultSpeed = 5.0f;
        public float psMaxSpeed = 25.0f;
        public float psMinSpeed = 1.0f;

        [Space]
        public float psTurnSwingBias = 32.0f;
        [Range(1, 180)]
        public float psTurnSwingMaxAngle = 90.0f;

        [Space]
        public float psCurrentSpeed = 0.0f;
        public float psCurrentBoost = 0.0f;
        public float psSpeedBreak = 0.5f;
        public float psCurrentTurn = -1.5f;
        [Space]
        public bool psUseAudioEffect = true;
        public AudioSource psAudioEffect;

        //---Debug
        public float psInternalGlobalSpeed = 0.0f;
        public float psInternalGlobalTurn = -1.5f;
        private int psInternalCurrentRound;

        private int psInternalNodeIndex;
        private bool psInternalPointBranchDone = false;

        private PathSystem_BezierSource psInternalBezierCurveBase;

        private float psInternalPreviousRotation;
        private Vector3 psInternalPreviousPosition;

        private Transform psInternalVirtualTrans;

        private void Start()
        {
            psInternalVirtualTrans = new GameObject("PathSystemVirtualObject").transform;
            psInternalVirtualTrans.hideFlags = HideFlags.HideInHierarchy;

            if (psCurrentBranch == null)
            {
                Debug.LogError("Path System_Object: Path branch is null");
                enabled = false;
                return;
            }

            if (psInternalBezierCurveBase == null)
            {
                psInternalBezierCurveBase = new PathSystem_BezierSource();
                psInternalBezierCurveBase.BezierQuality = psCurrentBranch.psBezierQuality;
            }

            if (!psUseAudioEffect)
                psAudioEffect = null;

            if(psSimulateOnStart)
                pathSys_Restart();
        }

        #region Public Accessible Functions

        /// <summary>
        /// Switch current path branch object (With smooth transition)
        /// </summary>
        public void pathSys_SwitchBranch(PathSystem_Branch targetBranch)
        {
            psCurrentBranch = targetBranch;
            psRefreshCurrentNode(true);
        }
        /// <summary>
        /// Refresh current audio (if enabled) & adjust it's volume by the object speed
        /// </summary>
        public void pathSys_AdjustAudioToActualSpeed(AudioSource aSource)
        {
            aSource.volume = psInternalGlobalSpeed / psMaxSpeed;
        }
        /// <summary>
        /// Set 'Look At Source' target (If Look At is enabled)
        /// </summary>
        public void pathSys_SetLookAtTarget(Transform _target)
        {
            psLookAtSource = _target;
        }

        /// <summary>
        /// Set path loop attribute
        /// </summary>
        public void pathSys_SetLoop(bool _value)
        {
            psLoopPath = _value;
        }

        /// <summary>
        /// Set path maximum rounds attribute (If Loop is disabled)
        /// </summary>
        public void pathSys_SetRounds(int _rounds)
        {
            psRounds = _rounds;
        }

        /// <summary>
        /// Restart current object on the specific path branch
        /// </summary>
        public void pathSys_Restart()
        {
            psRefreshCurrentNode(!psContinueInPath);

            if (!psContinueInPath)
                psInternalVirtualTrans.position = psCurrentBranch.psNodes[0].position;
            else
            {
                psInternalVirtualTrans.position = transform.position;
                Transform closestPoint = psCurrentBranch.psNodes.OrderBy(x => (x.position - transform.position).sqrMagnitude).FirstOrDefault();
                int e = 0;
                foreach(Transform t in psCurrentBranch.psNodes)
                {
                    if(closestPoint == t)
                    {
                        psInternalNodeIndex = e;
                        psCurrentNode = t;
                        psCurrentNodePosition = t.position;
                        break;
                    }
                    e++;
                }
            }
            psInternalCurrentRound = 0;

            if (psUseAudioEffect && psAudioEffect != null)
                psAudioEffect.Play();
            psUpdateMovement = true;
        }
        /// <summary>
        /// Resume  or Start current object on the specific path branch
        /// </summary>
        public void pathSys_Start()
        {
            if (psUseAudioEffect && psAudioEffect != null)
                psAudioEffect.Play();
            psUpdateMovement = true;
        }
        /// <summary>
        /// Stop current object on the specific path branch
        /// </summary>
        public void pathSys_Stop()
        {
            if (psUseAudioEffect && psAudioEffect != null)
                psAudioEffect.Stop();
            psUpdateMovement = false;
        }

        #endregion

        private void Update()
        {
            if (!psUpdateMovement)
                return;
            psProcessMovement();
        }

        //---Refresh current node index
        private void psRefreshCurrentNode(bool setIndexToZero = false)
        {
            if (psInternalNodeIndex >= psCurrentBranch.psNodes.Count - 1)
                setIndexToZero = true;

            if (psCurrentBranch.psSetToBezier == false)
            {
                if (setIndexToZero)
                    psInternalNodeIndex = 0;
                else
                    psInternalNodeIndex++;

                psCurrentNode = psCurrentBranch.psNodes[psInternalNodeIndex];
                psCurrentNodePosition = psCurrentNode.position;
                return;
            }

            if (psInternalPointBranchDone)
            {
                if (setIndexToZero)
                    psInternalNodeIndex = 0;
                else
                    psInternalNodeIndex++;
                psInternalPointBranchDone = false;
            }
            psInternalBezierCurveBase.NextIndex();

            Transform node0 = psCurrentBranch.psNodes[psInternalNodeIndex];
            Transform node1;
            if (psInternalNodeIndex + 1 < psCurrentBranch.psNodes.Count)
                node1 = psCurrentBranch.psNodes[psInternalNodeIndex + 1];
            else
                node1 = psCurrentBranch.psNodes[0];
            psCurrentNode = node0;
            PathSystem_BezierSource.BezierParams_ bp = new PathSystem_BezierSource.BezierParams_();
            bp.p1 = node0;
            bp.p2 = node1;
            bp.b = node0.GetChild(0);

            psCurrentNodePosition = psInternalBezierCurveBase.GetPoint(bp, ref psInternalPointBranchDone);
        }
        //---Update current object's transform
        private void psProcessMovement()
        {
            if (psCurrentNode == null)
                psRefreshCurrentNode(true);
            if (psCurrentBranch.psSetToBezier == false)
                psCurrentNodePosition = psCurrentNode.position;

            if (psCurrentNode.GetComponent<PathSystem_Node>())
            {
                psCurrentBoost = psCurrentNode.GetComponent<PathSystem_Node>().NodeBoost;
                psCurrentTurn = psCurrentNode.GetComponent<PathSystem_Node>().NodeTurnValue - 1.5f;
                if (psCurrentNode.GetComponent<PathSystem_Node>().NodeEvent != null)
                    psCurrentNode.GetComponent<PathSystem_Node>().NodeEvent.Invoke();
            }

            //---Calculating object's angle & converting to speed
            if (psUseGravity)
            {
                float xAngle = transform.localEulerAngles.x;
                xAngle = (xAngle > 180) ? xAngle -= 360 : xAngle;
                xAngle = (xAngle <= 0.5f) ? xAngle = 0.5f : xAngle;
                psCurrentSpeed = (xAngle / psSpeedBreak) + psCurrentBoost;
            }
            else
                psCurrentSpeed = (psDefaultSpeed / psSpeedBreak) + psCurrentBoost;

            psCurrentSpeed = (psCurrentSpeed < 0.1f) ? 0.1f : psCurrentSpeed;

            //---Calculating objects's rotation & converting to 'Node Turn'
            if (psUseNaturalAngularRotation && psCopyPathRotation)
            {
                float currentRotation = transform.localRotation.eulerAngles.y;
                currentRotation = (currentRotation > 180) ? currentRotation - 360 : currentRotation;

                float difference = (currentRotation - psInternalPreviousRotation);
                difference *= -1;
                float convertedSwingMaxVal = (1.5f / 180) * psTurnSwingMaxAngle;
                difference = Mathf.Clamp(difference, -convertedSwingMaxVal, convertedSwingMaxVal);
                if (Mathf.Abs(difference) > 2.0f)
                    difference = 0;
                psCurrentTurn = difference - 1.5f;
            }

            //---General speed calculations [Movement Speed & (if possible) Turn]
            psInternalGlobalSpeed = Mathf.Lerp(psInternalGlobalSpeed, psCurrentSpeed, Time.deltaTime * psSpeedTransition);
            psInternalGlobalSpeed = Mathf.Clamp(psInternalGlobalSpeed, psMinSpeed, psMaxSpeed); //---Clamp max speed

            psInternalGlobalTurn = Mathf.Lerp(psInternalGlobalTurn, psCurrentTurn, Time.deltaTime * psTurnTransition);

            //---General audio tweaker [if possible]
            if (psAudioEffect != null)
                pathSys_AdjustAudioToActualSpeed(psAudioEffect);

            //---Calculating positions
            Transform cn = psCurrentNode;
            psInternalVirtualTrans.position = Vector3.MoveTowards(psInternalVirtualTrans.position, psCurrentNodePosition, psInternalGlobalSpeed * Time.deltaTime);
            Vector3 myFuturePosition = psInternalVirtualTrans.position;

            //---Calculating offset position [radius & offset] - if possible
            if (psRadialType)
            {
                float XoffsetStorage = psPositionOffset.x;
                if (psInternalGlobalTurn < -1.5f)
                    XoffsetStorage = -XoffsetStorage;
                Vector3 circlePos = new Vector3(psRadius * Mathf.Cos(psInternalGlobalTurn) - XoffsetStorage, psRadius * 
                    Mathf.Sin(psInternalGlobalTurn) - psPositionOffset.y, psPositionOffset.z);
                circlePos = transform.TransformDirection(circlePos);
                myFuturePosition += circlePos;
            }
            else
                myFuturePosition = new Vector3(myFuturePosition.x + psPositionOffset.x, myFuturePosition.y + psPositionOffset.y, myFuturePosition.z + psPositionOffset.z);

            //---Finalizing main transform position
            transform.position = Vector3.Lerp(transform.position, myFuturePosition, psMovementSmooth * Time.deltaTime);

            psRotationOffset.x = (psRotationOffset.x == 0) ? 1 : psRotationOffset.x;
            psRotationOffset.y = (psRotationOffset.y == 0) ? 1 : psRotationOffset.y;
            psRotationOffset.z = (psRotationOffset.z == 0) ? 1 : psRotationOffset.z;

            //---Calculating rotations
            Quaternion finalRot;
            float zRotationFinal = (psInternalGlobalTurn + 1.5f);
            Vector3 PosDifference = (myFuturePosition - psInternalPreviousPosition);
            if (psCurrentSpeed<=psMinSpeed)
                PosDifference = (psCurrentNodePosition - psInternalVirtualTrans.position);
            Quaternion additionalRot = Quaternion.LookRotation(PosDifference);
            
            if (!psUseNaturalAngularRotation)
                zRotationFinal = additionalRot.z;

            if (psCopyPathRotation)
                finalRot = Quaternion.Euler(new Vector3(additionalRot.eulerAngles.x * psRotationOffset.x, additionalRot.eulerAngles.y * psRotationOffset.y, zRotationFinal * psRotationOffset.z));
            else
                finalRot = Quaternion.Euler(new Vector3(psRotationOffset.x, psRotationOffset.y, zRotationFinal * psRotationOffset.z));

            if(!psCopyPathRotation && psLookAt && psLookAtSource)
                finalRot = Quaternion.LookRotation(psLookAtSource.position - myFuturePosition) * Quaternion.Euler(psRotationOffset);

            //---Finalizing main transform rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, finalRot, psRotationSmooth * Time.deltaTime);

            if (psInternalNodeIndex >= psCurrentBranch.psNodes.Count)
            {
                if (psOnPathEnd != null)
                    psOnPathEnd.Invoke();
                if (psLoopPath || psInternalCurrentRound < psRounds)
                {
                    psRefreshCurrentNode(true);

                    if (!psAdaptToUpcomingNode)
                    {
                        psInternalVirtualTrans.position = psCurrentNodePosition;
                        psInternalVirtualTrans.rotation = psCurrentNode.rotation;
                        transform.rotation = psInternalVirtualTrans.rotation;
                        transform.position = psInternalVirtualTrans.position;
                    }
                    psInternalCurrentRound++;
                    return;
                }
                else
                {
                    if(psUseAudioEffect && psAudioEffect!=null)
                        psAudioEffect.Stop();
                    psUpdateMovement = false;
                    return;
                }
            }

            if (Vector3.Distance(psInternalVirtualTrans.position, psCurrentNodePosition) < psMinimumNodeDistance)
                psRefreshCurrentNode();

            //---Storage of the old previous rotation
            float convertedRot = transform.localRotation.eulerAngles.y;
            convertedRot = (convertedRot > 180) ? convertedRot - 360 : convertedRot;
            psInternalPreviousRotation = Mathf.Lerp(psInternalPreviousRotation, convertedRot, Time.deltaTime * psTurnSwingBias);
        }

        private void LateUpdate()
        {
            if (!psCopyPathRotation)
                return;
            psInternalPreviousPosition = transform.position;
        }
    }
}