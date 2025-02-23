using Godot;
using System;
using GodotLib.Utils;

namespace GodotLib
{
    public class CameraControl
    {
        private readonly Camera3D _camera;
        private readonly Node3D _cameraPivot;

        private float _distance;
        private float _pitch = 0, _yaw = 0, _roll = 0;
        private float _xOffset = 0, _yOffset = 0;
        private float _fov;

        private float _mouseXOffset = 0;
        private float _mouseYOffset = 0;

        private float _pitchRotationSpeed = 1;
        private float _yawRotationSpeed = 1;
        private float _rollRotationSpeed = 1;

        //[min,max] Pitch Angle
        private readonly float _maxPitchAngle = 30.0f;
        private readonly float _minPitchAngle = -90.0f;

        //[min,max] Mouse Offset 
        private readonly float _maxMouseOffset = 100.0f;
        private readonly float _minMouseOffset = -100.0f;

        //[min,max] Rotation Speed
        private readonly float _minRotationSpeed = 1;
        private readonly float _maxRotationSpeed = 100;


        public float Distance { get; set; }
        public float Pitch
        {
            get => _pitch;
            private set
            {
                _pitch = Math.Clamp(value, _minPitchAngle, _maxPitchAngle);
                applyRotation(_pitch, null, null);
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
                applyRotation(null, _yaw, null);
            }
        }
        public float Roll
        {
            get => _roll;
            set
            {
                _roll = value;
                applyRotation(null, null, _roll);
            }
        }

        public float CameraXOffset { get; set; } = 0;
        public float CameraYOffset { get; set; } = 0;
        public float CameraXOffsetSpeed { get; set; } = 5;
        public float CameraYOffsetSpeed { get; set; } = 10;

        public float FOV { get; set; }
        public float ZoomSpeed { get; set; }

        //Reverse x and y axis or not
        public bool YAxisReversed { get; set; } = false;
        public bool XAxisReversed { get; set; } = false;

        //Mouse xy offset 
        public float MouseXOffset
        {
            get => _mouseXOffset;
            set => _mouseXOffset = clampMouseOffset(value);
        }
        public float MouseYOffset
        {
            get => _mouseYOffset;
            set => _mouseYOffset = clampMouseOffset(value);
        }

        //Angle rotation speed
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

        public CameraControl(Camera3D camera, Node3D cameraPivot)
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
        }
        public CameraControl(Camera3D camera, Node3D cameraPivot, float distance)
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
            Distance = distance;
        }
        public CameraControl(Camera3D camera, Node3D cameraPivot, float distance, float pitch, float yaw, float roll)
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
            Distance = distance;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
        }
        public CameraControl(Camera3D camera, Node3D cameraPivot, float distance, float pitch, float yaw, float roll, float cameraXoffset, float cameraYoffset)
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
            Distance = distance;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
            CameraXOffset = cameraXoffset;
            CameraYOffset = cameraYoffset;
        }

        public void Process(double delta)
        {
            //Camera distance control
            applyDistance();

            //Camera rotation control
            if (MouseYOffset != 0)
            {
                float offset = (YAxisReversed ? -1 : 1) * MouseYOffset;
                Pitch += (float)delta * offset * PitchRotationSpeed;
                MouseYOffset = 0;
            }
            if (MouseXOffset != 0)
            {
                float offset = (XAxisReversed ? 1 : -1) * MouseXOffset;
                Yaw += (float)delta * offset * YawRotationSpeed;
                MouseXOffset = 0;
            }
            if (CameraXOffset != _camera.Position.X)
            {
                var positon = _camera.Position;
                positon.X = MathUtils.NonLinearInterpolation(positon.X, CameraXOffset, CameraXOffsetSpeed * (float)delta);
                _camera.Position = positon;
            }

        }

        private void applyRotation(float? pitch, float? yaw, float? roll)
        {
            Vector3 rotation = _cameraPivot.RotationDegrees;
            if (pitch.HasValue) rotation.X = pitch.Value;
            if (yaw.HasValue) rotation.Y = yaw.Value;
            if (roll.HasValue) rotation.Z = roll.Value;
            _cameraPivot.RotationDegrees = rotation;
        }
        private void applyDistance()
        {
            Vector3 position = _camera.Position;
            position.Z = Distance;
            _camera.Position = position;
        }

        public void UpdateMouseOffset(Vector2 offset)
        {
            if (offset.LengthSquared() >= 0.01)
            {
                MouseXOffset = offset.X;
                MouseYOffset = offset.Y;
            }
        }
        private float clampMouseOffset(float offset)
        {
            return Math.Clamp(offset, _minMouseOffset, _maxMouseOffset);
        }
        public Vector3 GetCameraForward()
        {
            return -_camera.GlobalBasis.Z.Normalized();
        }
        public Vector3 GetCameraBack()
        {
            return _camera.GlobalBasis.Z.Normalized();
        }
        public Vector3 GetCameraLeft()
        {
            return -_camera.GlobalBasis.X.Normalized();
        }
        public Vector3 GetCameraRight()
        {
            return _camera.GlobalBasis.X.Normalized();
        }

    }
}
