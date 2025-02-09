using Godot;
using System;
using System.Runtime.CompilerServices;
namespace Cam
{
    public class CameraAngleControl
    {
        private Camera3D camera;
        private float pitch = 0;
        private float yaw = 0;
        private float roll = 0;
        private float mouseRotationSpeed=1;
        public Vector2 mouseOffset;

        public float Pitch
        {
            get => pitch;
            set
            {
            }
        }

        public float Yaw
        {
            get => yaw;
            set
            {
            }
        }

        public float Roll
        {
            get => roll;
            set
            {
            }
        }

        // Mouse args
        private readonly float MAX_MOUSE_OFFSET = 100.0f;
        private readonly float MIN_MOUSE_OFFSET = -100.0f;
        private readonly float MOUSE_ROTATION_THR = 0.01f;
        private readonly float maxPitchAngle = 90;
        public bool IsFree { get; set; } = true;
        public bool IsMouseControl { get; set; } = false;
        public float MouseRotaionSpeed { get; set; } = 20.0f;
        public Vector2 MouseOffset { get; set; }

        public CameraAngleControl(Camera3D camera, float pitch, float yaw, float roll, float speed)
        {
            this.camera = camera;

        }
        private Vector2 NormalAxis(Vector2 axis)
        {
            var x = Math.Clamp(axis.X, MIN_MOUSE_OFFSET, MAX_MOUSE_OFFSET);
            var y = Math.Clamp(axis.Y, MIN_MOUSE_OFFSET, MAX_MOUSE_OFFSET);
            return new Vector2(x, y);
        }

        //Camera Function
        public void SetCameraPitch(float angle) //x axis
        {
            SetCameraAngle(angle, null, null);
            GD.Print($"Set Camera Pitch Angle: {angle}");
        }
        public void SetCameraYaw(float angle) //y axis
        {
            SetCameraAngle(null, angle, null);
            GD.Print($"Set Camera Yaw Angle: {angle}");
        }
        public void SetCameraRoll(float angle)//z axis
        {
            SetCameraAngle(null, null, angle);
            GD.Print($"Set Camera Roll Angle: {angle}");
        }
        public void SetCameraAngle(float? x, float? y, float? z)
        {
            var rotation = RotationDegrees;
            if (x.HasValue) rotation.X = x.Value;
            if (y.HasValue) rotation.Y = y.Value;
            if (z.HasValue) rotation.Z = z.Value;
            RotationDegrees = rotation;
        }
        public void InitCameraArgs()
        {
            if (CheckCamera())
            {
                SetCameraDistance(CameraDistance);
                SetCameraAngle(CameraPitchAngle, CameraYawAngle, CameraRollAngle);
            }
        }
        public void RestCamera(float angle, float pitch, float yaw, float roll)
        {
            SetCameraDistance(angle);
            SetCameraAngle(pitch, yaw, roll);
        }

        private float ClampAngle(float angle)
        {
            if (angle < -360.0f) angle += 360.0f;
            if (angle > 360.0f) angle -= 360.0f;
            return Math.Clamp(angle, MinAngle, MaxAngle);
        }
        //Mouse Function	
        private float GetMouseScrollFactor()
        {
            float val = MouseWhellScrollFactor;
            MouseWhellScrollFactor = 0;
            return val;
        }
        public void CameraRotation(double delta)
        {
            Vector2 rotation = (float)delta * MouseOffset * MouseRotaionSpeed;
            if (rotation.LengthSquared() >= MOUSE_ROTATION_THR && IsMouseControl)
            {
                SetCameraPitch(ClampAngle(rotation.Y));
                if (IsFree)
                {

                }
            }
        }
    }
}
