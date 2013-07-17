using System.Collections.Generic;
using System.Drawing;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;
using Point = System.Windows.Point;

namespace Lab3
{
    internal class Helper
    {
        public static Matrix<double> GetWarpMatrix(List<Point> fPoints, List<Point> sPoints)
        {
            var mainMatrix = GetMainMatrix(sPoints);
            var matrixA = GetMatrixA(mainMatrix, fPoints);
            var matrixB = GetMatrixB(mainMatrix, fPoints);
            var resultMatrix = new DenseMatrix(2, 3);
            resultMatrix.SetRow(0, matrixA.Column(0));
            resultMatrix.SetRow(1, matrixB.Column(0));

            return resultMatrix;
        }

        private static Matrix<double> GetMainMatrix(List<Point> fPoints)
        {
            var fmatrix = new DenseMatrix(3, 3);
            fmatrix.SetRow(0, new[] { 1, fPoints[0].X, fPoints[0].Y });
            fmatrix.SetRow(1, new[] { 1, fPoints[1].X, fPoints[1].Y });
            fmatrix.SetRow(2, new[] { 1, fPoints[2].X, fPoints[2].Y });
            
            return fmatrix.Inverse();
        }

        private static Matrix<double> GetMatrixA(Matrix<double> mainMatrix, List<Point> sPoints)
        {
            var sMatrix = new DenseMatrix(3, 1);
            sMatrix.SetColumn(0, new[] { sPoints[0].X, sPoints[1].X, sPoints[2].X });

            return mainMatrix * sMatrix;
        }

        private static Matrix<double> GetMatrixB(Matrix<double> mainMatrix, List<Point> sPoints)
        {
            var sMatrix = new DenseMatrix(3, 1);
            sMatrix.SetColumn(0, new[] { sPoints[0].Y, sPoints[1].Y, sPoints[2].Y });

            return mainMatrix * sMatrix;
        }

        public static Bitmap CreatePicture(Bitmap bmp, Matrix<double> warpMatrix)
        {
            var resultBmp = new Bitmap(bmp.Width, bmp.Height);
            for (int i = 0; i < bmp.Width; i++)
            {
                for (int j = 0; j < bmp.Height; j++)
                {
                    var xNew = warpMatrix[0, 0] + warpMatrix[0, 1] * i + warpMatrix[0, 2] * j;
                    var yNew = warpMatrix[1, 0] + warpMatrix[1, 1] * i + warpMatrix[1, 2] * j;
                    if (xNew < 0 || xNew >= bmp.Width || yNew < 0 || yNew >= bmp.Height) continue;
                    var color = bmp.GetPixel((int)xNew, (int)yNew);
                    resultBmp.SetPixel(i, j, color);
                }
            }
            return resultBmp;
        }

        public static void BilinearFiltering(Bitmap bmp)
        {
        }
    }
}
