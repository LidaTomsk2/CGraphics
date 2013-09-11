using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Lab2.DrawElements.Lines
{
    public class LineSimple : LineFigure
    {
        public event EventHandler SplitPressEventHandler;

        public LineSimple() : base()
        {
            MouseLeftButtonDown += OnMouseLeftButtonDown;
        }

        private void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt))
            {
                SplitPressEventHandler.Invoke(this, e);
            }
        }

        protected override void DrawGeometry(StreamGeometryContext context)
        {
            context.BeginFigure(FromPoint, true, false);
            context.LineTo(TargetPoint, true, true);
        }
    }
}
