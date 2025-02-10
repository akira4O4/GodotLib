using Godot;
using System;
using System.Runtime.CompilerServices;
using Math
/*
Node3D (Target Node)
    |
    -Camera3D
*/

namespace CameraControl
{
    public class CameraAngleControl
    {
        private Camera3D _camera;
        private Node3D _targetNode;
        private float _pitch = 0;
        private float _yaw = 0;
        private float _roll = 0;
        private float _pitchRotationSpeed = 1;
        private float _yawRotationSpeed = 1;
        private float _rollRotationSpeed = 1;

        private Vector2 _mouseOffset;

        //min max rotation speed 
        private readonly float _minRotationSpeed = 1;
        private readonly float _maxRotationSpeed = 100;

        //min max pitch rotation angle
        private readonly float _maxPitchAngle = 90.0f;
        private readonly float _minPitchAngle = -45.0f;

        //min max mouse offset
        private readonly float _maxMouseOffset = 100.0f;
        private readonly float _minMouseOffset = -100.0f;

        private readonly float _minRotationThr = 0.01f;

        //View control mode
        private bool _freeView = true;
        private bool _controlView = !_freeView;

        public float Pitch
        {
            get => _pitch;
            private set => _pitch = clampAngle(value, _minPitchAngle, _maxPitchAngle);
        }
        public float Yaw
        {
            get => _yaw;
            private set => _yaw = value;
        }
        public float Roll
        {
            get => _roll;
            private set => _roll = value;
        }

        public bool FreeView
        {
            get { return _freeView; }
            set
            {
                _freeView = value;
                _controlView = !value;
            }
        }
        public bool ControlView
        {
            get { return _controlView; }
            set
            {
                _freeView = !value;
                _controlView = value;
            }
        }

        public Vector2 MouseOffset
        {
            get => _mouseOffset;
            set => _mouseOffset = clampOffset(value);
        }

        public float PitchRotationSpeed
        {
            get => _pitchRotationSpeed;
            set => _pitchRotationSpeed = Math.Clamp(value, _minRotationSpeed, _maxRotationSpeed);
        }
        public float YawRotationSpeed
        {
            get => _yawRotationSpeed;
            set => _yawRotationSpeed = Math.Clamp(value, _minRotationSpeed, _maxRotationSpeed);
        }
        public float RollRotationSpeed
        {
            get => _rollRotationSpeed;
            set => _rollRotationSpeed = Math.Clamp(value, _minRotationSpeed, _maxRotationSpeed);
        }

        public CameraAngleControl(Node3D targetNode, float pitch, float yaw, float roll)
        {
            _targetNode = targetNode;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
            applyRotation(Pitch,Yaw,Roll);
        }
        private float clampAngle(float angle, minAngle, maxAngle)
        {
            if (angle < -360.0f) angle += 360.0f;
            if (angle > 360.0f) angle -= 360.0f;
            return Math.Clamp(angle, minAngle, maxAngle);
        }
        private Vector2 clampOffset(Vector2 offset)
        {
            var x = Math.Clamp(offset.X, _minMouseOffset, _maxMouseOffset);
            var y = Math.Clamp(offset.Y, _minMouseOffset, _maxMouseOffset);
            return new Vector2(x, y);
        }
        private void applyRotation(float? pitch, float? yaw, float? roll)
        {
            var rotation = _targetNode.RotationDegrees;
            if (pitch.HasValue) rotation.X = pitch.Value;
            if (yaw.HasValue) rotation.Y = yaw.Value;
            if (roll.HasValue) rotation.Z = roll.Value;
            _targetNode.RotationDegrees = rotation;
        }
        public void ResetRotation()
        {
            applyRotation(0, 0, 0);
        }
        public void SetPitch(float angle)
        {
            Pitch = angle;
            applyRotation(angle, null, null);
        }
        public void SetYaw(float angle)
        {
            Yaw = angle;
            applyRotation(null, angle, null);
        }
        public void SetRoll(float angle)
        {
            Roll = angle;
            applyRotation(null, null, angle);
        }
        public void AddPitch(float delta)
        {
            Pitch += delta * PitchRotationSpeed;
            applyRotation(Pitch, null, null);
        }
        public void AddYaw(float delta)
        {
            Yaw += delta * YawRotationSpeed;
            applyRotation(null, Yaw, null);
        }
        public void AddRoll(float delta)
        {
            Roll += delta * RollRotationSpeed;
            applyRotation(null, null, Roll);
        }
    }
}
