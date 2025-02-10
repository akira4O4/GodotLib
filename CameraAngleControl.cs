using Godot;
using System;

namespace CameraControl
{
    public class CameraAngleControl
    {
        // private Camera3D _camera;
        private Node3D _targetNode;
        private float _pitch = 0;
        private float _yaw = 0;
        private float _roll = 0;
        private float _pitchRotationSpeed = 1;
        private float _yawRotationSpeed = 1;
        private float _rollRotationSpeed = 1;

        //min max rotation speed 
        private readonly float _minRotationSpeed = 1;
        private readonly float _maxRotationSpeed = 100;

        //min max pitch rotation angle
        private readonly float _maxPitchAngle = 45.0f;
        private readonly float _minPitchAngle = -90.0f;
        private readonly float _maxRollAngle = 45.0f;
        private readonly float _minRollAngle = -45.0f;

        //min max mouse offset
        private readonly float _maxMouseOffset = 100.0f;
        private readonly float _minMouseOffset = -100.0f;
        private bool _freeView = true;
        private bool _controlView = false;
        public bool YAxisReversed { get; set; } = false;
        public bool XAxisReversed { get; set; } = false;
        public float Pitch
        {
            get => _pitch;
            private set
            {
                _pitch = Math.Clamp(value, _minPitchAngle, _maxPitchAngle);
            }
        }
        public float Yaw
        {
            get => _yaw;
            private set
            {
                if (value < 0)
                    _yaw = -((-value) % 360.0f);
                else
                    _yaw = value % 360.0f;
            }
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
                _controlView = value;
                _freeView = !value;
            }
        }
        private float _mouseOffsetX = 0;
        public float MouseOffsetX
        {
            get => _mouseOffsetX;
            set => _mouseOffsetX = clampOffset(value);
        }
        private float _mouseOffsetY = 0;
        public float MouseOffsetY
        {
            get => _mouseOffsetY;
            set => _mouseOffsetY = clampOffset(value);
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
            applyRotation(Pitch, Yaw, Roll);
        }
        private float clampOffset(float offset)
        {
            return Math.Clamp(offset, _minMouseOffset, _maxMouseOffset);
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
        public void UpdatePitch(float delta)
        {

            if (MouseOffsetY != 0)
            {
                float y_offset = (YAxisReversed ? -1 : 1) * MouseOffsetY;
                Pitch += delta * y_offset * PitchRotationSpeed;
                applyRotation(Pitch, null, null);
                MouseOffsetY = 0;
            }

        }
        public void UpdateYaw(float delta)
        {
            if (MouseOffsetX != 0)
            {
                float x_offset = (XAxisReversed ? 1 : -1) * MouseOffsetX;
                Yaw += delta * x_offset * YawRotationSpeed;
                applyRotation(null, Yaw, null);
                MouseOffsetX = 0;
            }
        }
        public void UpdateRoll(float delta)
        {
            // Roll += delta * RollRotationSpeed;
            // applyRotation(null, null, Roll);
        }
    }
}
