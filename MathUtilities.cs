using System;

namespace LFE.KeyboardShortcuts
{
    public static class MathUtilities
    {
        public static bool SameSign(float a, float b)
        {
            // I am considering 0 "neutral" and matches sign of the other param
            return a * b >= 0.0f;
        }
    }
}
