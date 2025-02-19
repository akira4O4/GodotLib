using Godot;
namespace GodotLib.Utils
{
    public static class Utils
    {
        public static void ChangeNodeRotationDegrees(Node3D node, Vector3 newDegrees, bool ignoreX = false, bool ignoreY = false, bool ignoreZ = false)
        {
            Vector3 targetDegrees = node.RotationDegrees;
            if (targetDegrees == newDegrees)
                return;

            if (!ignoreX)
                targetDegrees.X = newDegrees.X;
            if (!ignoreY)
                targetDegrees.Y = newDegrees.Y;
            if (!ignoreZ)
                targetDegrees.Z = newDegrees.Z;

            node.RotationDegrees=targetDegrees;     
        
        }
    }
}