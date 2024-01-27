namespace NuiN.NExtensions
{
    public static class NumberExtensions
    {
        public static bool InRange(this float value, float min, float max)
        {
            return value >= min && value <= max;
        }
    }
}