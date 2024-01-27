using UnityEngine;

namespace NuiN.NExtensions
{
    public static class ColorExtensions
    {
        public static Color WithAlpha(this Color color, float alpha) 
            => new (color.r, color.g, color.b, alpha);
    }
}
