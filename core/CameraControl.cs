using Godot;
using System;
using GodotLib.Utils;
using Godot.NativeInterop;
using Microsoft.Win32.SafeHandles;
using System.Threading;

namespace GodotLib
{
    public class CameraControl
    {
        private readonly Camera3D _camera;
        private readonly Node3D _cameraPivot;

        private float _fov;
        private float _distance;
        private float _pitch = 0, _yaw = 0, _roll = 0;
        private float _distanceZoomSpeed = 10;

        private float _cameraXOffset = 0;
        private float _cameraYOffset = 0;

        private float _cameraXOffsetSpeed = 5;
        private float _cameraYOffsetSpeed = 5;

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

        public float FOV
        {
            get => _fov;
            set => _fov = value;
        }

        public float Distance
        {
            get => _distance;
            set => _distance = value;
        }
        public float DistanceZoomSpeed
        {
            get => _distanceZoomSpeed;
            set => _distanceZoomSpeed = value;
        }

        public float Pitch
        {
            get => _pitch;
            set => _pitch = Math.Clamp(value, _minPitchAngle, _maxPitchAngle);
        }
        public float Yaw
        {
            get => _yaw;
            set
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
            set => _roll = value;
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

        public float CameraXOffset
        {
            get => _cameraXOffset;
            set => _cameraXOffset = value;
        }
        public float CameraYOffset
        {
            get => _cameraYOffset;
            set => _cameraYOffset = value;
        }
        public float CameraXOffsetSpeed
        {
            get => _cameraXOffsetSpeed;
            set => _cameraXOffsetSpeed = value;
        }
        public float CameraYOffsetSpeed
        {
            get => _cameraYOffsetSpeed;
            set => _cameraYOffsetSpeed = value;
        }

        public bool YAxisReversed { get; set; } = false;
        public bool XAxisReversed { get; set; } = false;

        //Mouse xy offset 
        public float MouseXOffset
        {
            get
            {
                float offset = _mouseXOffset;
                _mouseXOffset = 0;
                return offset;
            }
            set => _mouseXOffset = clampMouseOffset(value);
        }
        public float MouseYOffset
        {
            get
            {
                float offset = _mouseYOffset;
                _mouseYOffset = 0;
                return offset;
            }
            set => _mouseYOffset = clampMouseOffset(value);
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
            applyDistance();
        }
        public CameraControl(Camera3D camera, Node3D cameraPivot, float distance, float pitch, float yaw, float roll)
        {
            _camera = camera;
            _cameraPivot = cameraPivot;
            Distance = distance;
            Pitch = pitch;
            Yaw = yaw;
            Roll = roll;
            applyRotation();
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
            applyDistance();
            applyRotation();
        }

        public void Process(double delta)
        {

            if (_camera.Position.Z != _distance)
            {
                float _d = MathUtils.NonLinearInterpolation(_camera.Position.Z, _distance, _distanceZoomSpeed * (float)delta);
                applyDistance(_d);
            }

            //Camera rotation control
            if (_mouseYOffset != 0)
            {
                float offset = (YAxisReversed ? -1 : 1) * MouseYOffset;
                Pitch += (float)delta * offset * _pitchRotationSpeed;
                applyRotation();
            }
            if (_mouseXOffset != 0)
            {
                float offset = (XAxisReversed ? 1 : -1) * MouseXOffset;
                Yaw += (float)delta * offset * _yawRotationSpeed;
                applyRotation();
            }

            // if (_cameraXOffset != _camera.Position.X)
            // {
            //     var positon = _camera.Position;
            //     positon.X = MathUtils.NonLinearInterpolation(positon.X, _cameraXOffset, _cameraXOffsetSpeed * (float)delta);
            //     _camera.Position = positon;
            // }

        }

        // private void applyPitch(float? angle = null)
        // {
        //     float _angle = angle.HasValue ? angle.Value : _pitch;
        //     applyRotation(pitch:_angle);
        // }
        // private void applyYaw(float? angle = null)
        // {
        //     float _angle = angle.HasValue ? angle.Value : _pitch;
        //     applyRotation(yaw: _angle);
        // }
        // private void applyRoll(float? angle = null)
        // {
        //     float _angle = angle.HasValue ? angle.Value : _pitch;
        //     applyRotation(roll: _angle);
        // }
        private void applyDistance(float? distance = null)
        {
            Vector3 position = _camera.Position;
            position.Z = distance.HasValue ? distance.Value : _distance;
            _camera.Position = position;
        }
        private void applyRotation(float? pitch = null, float? yaw = null, float? roll = null)
        {
            if(_camera.RotationDegrees==new Vector3(_pitch,_yaw,_roll))
            {
                return;
            } 
            Vector3 rotation = _cameraPivot.RotationDegrees;
             if(_camera.RotationDegrees!=_pitch)
             {
                  rotation.X = pitch.Value;
             }
            rotation.X = pitch.HasValue ? pitch.Value : _pitch;
            rotation.Y = yaw.HasValue ? yaw.Value : _yaw;
            rotation.Z = roll.HasValue ? roll.Value : _roll;
            // if (pitch.HasValue) rotation.X = pitch.Value;
            // if (yaw.HasValue) rotation.Y = yaw.Value;
            // if (roll.HasValue) rotation.Z = roll.Value;
            _cameraPivot.RotationDegrees = rotation;
        }

        public void UpdateMouseOffset(Vector2 offset)
        {
            if (offset.LengthSquared() >= 0.1)
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
