using Godot;
using System;
namespace CameraControl
{
    public class CameraDistanceControal
    {
        private float _speed = 1;
        private float _distance = 10;
        private readonly Camera3D _camera;
        private readonly float _minDistance = 0;
        private readonly float _maxDistance = 10;
        private readonly float _minSpeed = 1;
        private readonly float _maxSpeed = 10;

        public float Distance
        {
            get => _distance;
            private set => _distance = Math.Clamp(value, _minDistance, _maxDistance);
        }
        public float Speed
        {
            get => _speed;
            set => _speed = Math.Clamp(value, _minSpeed, _maxSpeed);
        }
        public CameraDistanceControal(Camera3D camera, float distance)
        {
            if (camera is null)
            {
                GD.PrintErr("camera is null");
            }
            else
            {
                _camera = camera;
                Distance = distance;
                SetDistance(Distance);
            }
        }
        private void applyDistance()
        {
            Vector3 position = _camera.Position;
            position.Z = Distance;
            _camera.Position = position;
        }
        public void SetDistance(float distance)
        {
            Distance = distance;
            applyDistance();
        }
        public void AddDistance(float delta)
        {
            Distance += delta * Speed;
            applyDistance();
        }
    }
}
