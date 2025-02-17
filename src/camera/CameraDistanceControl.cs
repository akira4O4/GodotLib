using Godot;
using System;

namespace GodotLib.Camera
{
    enum CameraDistance{
        Outside=10,
        Inside=3,
    }
    public class CameraDistanceControl
    {
        private float _speed = 1.0f;
        private float _distance = 10.0f;
        private float _targetDistance = 0.0f;
        private readonly Camera3D _camera;
        private readonly RayCast3D _rayCast;
        public float MinSpeed { get; set; } = 1;
        public float MaxSpeed { get; set; } = 10;
        public float MinDistance { get; set; } = 1;
        public float MaxDistance { get; set; } = 30;

        public float Distance
        {
            get => _distance;
            set
            {
                _distance = clampDistance(value);
                applyDistance();//Set distance right new
            }
        }
        public float TargetDistance
        {
            get => _targetDistance;
            set => _targetDistance = clampDistance(value);
        }
        public float Speed
        {
            get => _speed;
            set => _speed = Math.Clamp(value, MinSpeed, MaxSpeed);
        }

        public CameraDistanceControl(Camera3D camera, float distance, RayCast3D rayCast)
        {
            _camera = camera;
            _rayCast = rayCast;
            Distance = TargetDistance = distance;
        }
        private float clampDistance(float distance)
        {
            return Math.Clamp(distance, MinDistance, MaxDistance);
        }
        private void applyDistance()
        {
            Vector3 position = _camera.Position;
            position.Z = Distance;
            _camera.Position = position;
        }

        //Ray-->Camera
        private void UpdateRayCastDirection()
        {
            _rayCast.TargetPosition = _camera.Position;
        }
        private void rayCollisionDetection()
        {
            UpdateRayCastDirection();
            if (_rayCast.IsColliding())
            {
                Vector3 collisionPoint = _rayCast.GetCollisionPoint();
                
                TargetDistance=_rayCast.ToLocal(collisionPoint).Z;
            }
        }
        public void ProcessDistance(double delta)
        {
            rayCollisionDetection();
            if (Distance!=TargetDistance)
            {
                float t = 1 - Mathf.Exp(-Speed * (float)delta);
                Distance = Mathf.Lerp(Distance, TargetDistance, t);
            }
        }
    }
}
