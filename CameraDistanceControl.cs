using Godot;
using System;

namespace CameraControl
{
    public class CameraDistanceControl
    {
        private float _speed = 1;
        private float _distance = 10;
        private float _targetDistance = 0;

        private readonly Camera3D _camera;
        partial readonly RayCast3D _rayCast;
        private readonly float _minSpeed = 1;
        private readonly float _maxSpeed = 10;
        private readonly float _minDistance = 0;
        private readonly float _maxDistance = 10;

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
            private get => _targetDistance;
            set => _targetDistance = clampDistance(value);
        }
        public float Speed
        {
            get => _speed;
            set => _speed = Math.Clamp(value, _minSpeed, _maxSpeed);
        }

        public CameraDistanceControl(Camera3D camera, float distance, RayCast3D rayCast)
        {
            _camera = camera;
            _rayCast = rayCast;
            Distance = TargetDistance = distance;
            _rayCast.CastTo = _camera.GlobalPosition - _rayCast.GlobalPosition;
        }
        private float clampDistance(float distance)
        {
            return Math.Clamp(distance, _minDistance, _maxDistance);
        }
        private void applyDistance()
        {
            Vector3 position = _camera.Position;
            position.Z = Distance;
            _camera.Position = position;
        }
        public void Process(double delta)
        {
            if (_rayCast.IsColliding())
            {
                var collider = _rayCast.GetCollider();

                if (collider != _camera)
                {
                    Vector3 collisionPoint = _rayCast.GetCollisionPoint();
                    Distance = TargetDistance = collisionPoint.Z;
                }

                if (Distance != TargetDistance)
                {
                    float t = 1 - Mathf.Exp(-Speed * (float)delta);
                    Distance = Mathf.Lerp(Distance, TargetDistance, t);
                }
            }
        }
    }
}
