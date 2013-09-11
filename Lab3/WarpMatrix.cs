using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace Lab3
{
    public class WarpMatrix
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
    }
}
