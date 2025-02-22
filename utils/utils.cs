using Godot;
namespace GodotLib.Utils
{
    public static class Utils
    {
        public static void SetNodeRotationDegrees(Node3D node, Vector3 newDegrees, bool ignoreX = false, bool ignoreY = false, bool ignoreZ = false)
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

            node.RotationDegrees = targetDegrees;

        }
        public static void SetNodePitch(Node3D node, float degree)
        {
            if (node == null) return;
            if (degree == 0) return;
            Vector3 targetDegrees = node.RotationDegrees;
            targetDegrees.X = degree;
            node.RotationDegrees = targetDegrees;
        }
        public static void SetNodeYaw(Node3D node, float degree)
        {
            if (node == null) return;
            if (degree == 0) return;
            Vector3 targetDegrees = node.RotationDegrees;
            targetDegrees.Y = degree;
            node.RotationDegrees = targetDegrees;
        }

        public static void SetNodeRoll(Node3D node, float degree)
        {
            if (node == null) return;
            if (degree == 0) return;
            Vector3 targetDegrees = node.RotationDegrees;
            targetDegrees.Z = degree;
            node.RotationDegrees = targetDegrees;

        }
    }
}