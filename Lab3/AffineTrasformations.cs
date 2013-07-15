using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3
{
    internal class AffineTrasformations
    {
        public static Matrix GetWarpMatrix(List<Point> fPoints, List<Point> sPoints)
        {
            var fMatrix = new Matrix((float)fPoints[0].X, (float)fPoints[0].Y, (float)fPoints[1].X,
                (float)fPoints[1].Y, (float)fPoints[2].X, (float)fPoints[2].Y);
            var sMatrix = new Matrix((float)sPoints[0].X, (float)sPoints[0].Y, (float)sPoints[1].X,
                (float)sPoints[1].Y, (float)sPoints[2].X, (float)sPoints[2].Y);

            fMatrix.Invert();
            sMatrix.Multiply(fMatrix);

            return sMatrix;
        }
    }
}
