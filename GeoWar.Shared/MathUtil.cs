using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    static class MathUtil
    {
        /// <summary>
        /// generates a vector from a given angle and magnitude, the greater the magnitude the larger
        /// the vector length.SOhCoHToa to determine x and y values of the vector. The magnitide is needed
        /// for adjusting the speed of the bullets
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="magnitude"></param>
        /// <returns></returns>
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
