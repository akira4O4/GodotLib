using Godot;
namespace GodotLib.Utils
{
    public class CameraUtils
    {
        readonly Camera3D _camera;
        public CameraUtils(Camera3D camera)
        {
            _camera = camera;
        }
        public Vector3 GetForward()
        {
            return -_camera.GlobalBasis.Z.Normalized();
        }
        public Vector3 GetBack()
        {
            return _camera.GlobalBasis.Z.Normalized();
        }
        public Vector3 GetLeft()
        {
            return -_camera.GlobalBasis.X.Normalized();
        }
        public Vector3 GetRight()
        {
            return _camera.GlobalBasis.X.Normalized();
        }
        public Vector3 GetRotationDegrees()
        {
            return _camera.GlobalRotationDegrees;
        }
    }
}