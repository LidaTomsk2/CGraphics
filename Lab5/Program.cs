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
            var aa_modes = new List<int>();
            int aa = 0;
            do
            {
                var mode = new GraphicsMode(32, 0, 0, aa);
                if (!aa_modes.Contains(mode.Samples))
                    aa_modes.Add(aa);
                aa += 2;
            } while (aa <= 32);

            using (MainWindow win = new MainWindow(aa_modes[aa_modes.Count - 1]))
            {
                win.Run();
            }
        }
    }
}
