using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;

namespace Lab5
{
    class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            var aaModes = new List<int>();
            var aa = 0;
            do
            {
                var mode = new GraphicsMode(32, 0, 0, aa);
                if (!aaModes.Contains(mode.Samples))
                    aaModes.Add(aa);
                aa += 2;
            } while (aa <= 32);

            using (var win = new MainWindow(aaModes[aaModes.Count - 1]))
            {
                win.Run();
            }
        }
    }
}
