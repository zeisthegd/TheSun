using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Penwyn.Game;

namespace Penwyn.Tools
{
    public class CameraMouseControl : SingletonMonoBehaviour<CameraMouseControl>
    {
        public float MoveSensitivity = 0.1F;
        public float RotateSensitivity = 0.1F;
        public float ZoomSensitivity = 0.1F;
        public float Distance = 4;
        public LayerMask BlockZoomMask;

        public float ConstraintHeight = 4;
        public FloatMinMax ZoomDistance = new FloatMinMax(1, 4);

        public Camera Camera;
        public GameObject LookAt;

        public bool Horizontal = true;
        public bool Vertical = true;

        private Vector2 _moveInput;
        private float _xRotation = 0;

        private StateMachine<LockMode> _rotateLockMode = new StateMachine<LockMode>(LockMode.UNLOCKED);
        private StateMachine<LockMode> _zoomLockMode = new StateMachine<LockMode>(LockMode.UNLOCKED);
        private StateMachine<RotateState> _movementStates = new StateMachine<RotateState>(RotateState.IDLE);

        public StateMachine<LockMode> RotateLockMode { get => _rotateLockMode; }
        public StateMachine<LockMode> ZoomLockMode { get => _zoomLockMode; }
        public StateMachine<RotateState> MovementState { get => _movementStates; }

        private void Update()
        {
            RotateCamera();
            MoveCamera();
        }

        public void Position()
        {
            Vector3 dir = (Camera.transform.position - LookAt.transform.position).normalized;
            Camera.transform.position = dir * Distance;
        }

        public void RotateCamera()
        {
            if (_rotateLockMode.Is(LockMode.UNLOCKED) && _movementStates.Is(RotateState.STARTED) && IsMovingHorizontal())
            {
                if (Horizontal)
                    Camera.transform.RotateAround(LookAt.transform.position,
                                        Vector3.up,
                                        _moveInput.x * RotateSensitivity * Time.deltaTime);

                if (Vertical)
                    Camera.transform.RotateAround(LookAt.transform.position,
                                        Vector3.right,
                                        _moveInput.y * RotateSensitivity * Time.deltaTime);

                Camera.transform.eulerAngles = new Vector3(Camera.transform.eulerAngles.x, Camera.transform.eulerAngles.y, 0);
            }
        }

        public void MoveCamera()
        {
            if (_movementStates.Is(RotateState.STARTED) && IsMovingVertical())
            {
                Camera.transform.position += (-_moveInput.y * Vector3.up * MoveSensitivity);
                if (!IsInsideTargetHeight())
                {
                    Camera.transform.position -= (-_moveInput.y * Vector3.up * MoveSensitivity);
                }
            }
        }

        private void StartRotate()
        {
            _movementStates.Change(RotateState.STARTED);
        }

        private void StopRotate()
        {
            _movementStates.Change(RotateState.IDLE);
        }


        private void OnMouseMove(Vector2 vector2)
        {
            _moveInput = vector2;
        }

        private void OnMouseZoomClose()
        {
            if (_zoomLockMode.Is(LockMode.UNLOCKED))
            {
                Vector3 dir = (LookAt.transform.position - Camera.transform.position).normalized;
                if (Vector3.Distance(Camera.transform.position + (dir * ZoomSensitivity), LookAt.transform.position) > ZoomDistance.Min)
                    Camera.transform.position += dir * ZoomSensitivity;
            }
        }

        private void OnMouseZoomFar()
        {
            if (_zoomLockMode.Is(LockMode.UNLOCKED))
            {
                Vector3 dir = (Camera.transform.position - LookAt.transform.position).normalized;
                if (Vector3.Distance(Camera.transform.position + (dir * ZoomSensitivity), LookAt.transform.position) < ZoomDistance.Max)
                    Camera.transform.position += dir * ZoomSensitivity;
            }
        }

        private bool IsInsideTargetHeight()
        {
            return Mathf.Abs(Camera.transform.position.y - LookAt.transform.position.y) < ConstraintHeight;
        }

        private bool IsMovingHorizontal()
        {
            return Mathf.Abs(_moveInput.x) > Mathf.Abs(_moveInput.y);
        }

        private bool IsMovingVertical()
        {
            return Mathf.Abs(_moveInput.y) > Mathf.Abs(_moveInput.x);
        }

        protected void OnEnable()
        {
        }

        protected void OnDisable()
        {
    
        }
    }

    public enum LockMode
    {
        LOCKED,
        UNLOCKED
    }

    public enum RotateState
    {
        IDLE,
        STARTED
    }
}

