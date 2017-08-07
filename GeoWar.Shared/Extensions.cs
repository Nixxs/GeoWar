using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoWar
{
    static class Extensions
    {
        /// <summary>
        /// this function takes a vector2 and returns the angle as radians of the vector 2
        /// think about it as 0,0 the top left corner, the x value along the horizontal axies
        /// and the y value along the vertical axis. The angle is angle of the line that forms between
        /// 0,0 and the interection between x and y lines
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
    }
}
