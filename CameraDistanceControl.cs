using Godot;
using System;
namespace Cam
{
    public class CameraDistanceControal
    {
        private float speed = 1;
        private float distance = 10;
        private readonly Camera3D camera;
        private readonly float minDistance = 2;
        private readonly float maxDistance = 30;
        private readonly float minSpeed = 1;
        private readonly float maxSpeed = 10;

        public float Distance
        {
            get => distance;
            private set => distance = Math.Clamp(value, minDistance, maxDistance);
        }
        public float Speed
        {
            get => speed;
            private set => speed = Math.Clamp(value, minSpeed, maxSpeed);
        }
        public CameraDistanceControal(Camera3D camera, float distance, float speed)
        {
            if (camera is null)
            {
                GD.PrintErr("camera is null");
            }
            else
            {
                this.camera = camera;
                Distance = distance;
                Speed = speed;
                SetDistance(Distance);
            }
        }

        private void ApplyDistance()
        {
            Vector3 position = camera.Position;
            position.Z = Distance;
            camera.Position = position;
        }
        public void SetDistance(float distance)
        {
            Distance = distance;
            ApplyDistance();
        }

        public void AddDistance(float delta)
        {
            Distance += delta * Speed;
            ApplyDistance();
            GD.Print($"Set Camera Distance: {Distance}");
        }
    }
}
