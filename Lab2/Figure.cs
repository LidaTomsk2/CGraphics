using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Lab2
{
    public class Figure : Shape
    {
        protected override Geometry DefiningGeometry
        {
            get { throw new NotImplementedException(); }
        }
    }
}
