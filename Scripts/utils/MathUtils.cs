using Godot;
namespace GodotLib.Utils
{
    public static class MathUtils
    {
        public static float NonLinearInterpolation(float from, float to, float weight)
        {
            float t = 1 -Mathf.Exp(-weight);
            return Mathf.Lerp(from, to, t);
        }
        
    }

}