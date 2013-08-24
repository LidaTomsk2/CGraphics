using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xml.Serialization;

namespace Lab4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IEnumerable<LSystemModel> _systems;
        public MainWindow()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            CbSystems.SelectionChanged += CbSystemsOnSelectionChanged;
        }

        private void CbSystemsOnSelectionChanged(object sender, SelectionChangedEventArgs selectionChangedEventArgs)
        {
            var system = _systems.First(x => x.Name == (string)CbSystems.SelectedValue);
            TbAngle.Text = system.Angle.ToString();
            TbDeepValue.Text = system.DeepValue.ToString();
            TbStepSize.Text = system.StepSize.ToString();
        }

        private void OnLoaded(object sender, RoutedEventArgs routedEventArgs)
        {
            _systems = LParser.ParseFiles(@"../../L-Systems");
            foreach (var lSystemModel in _systems)
            {
                CbSystems.Items.Add(lSystemModel.Name);
            }
            CbSystems.SelectedIndex = 0;
        }

        private void BtnStartOnClick(object sender, RoutedEventArgs e)
        {
            var angle = Convert.ToDouble(TbAngle.Text);
            var stepSize = Convert.ToInt32(TbStepSize.Text);
            var deepValue = Convert.ToInt32(TbDeepValue.Text);

            DrawCanvas.Children.Clear();
            var rules = _systems.First(x => x.Name == (string) CbSystems.SelectedValue).Rules;
            var strToDraw = LParser.GetCombinedParsedString(rules, deepValue);
            var fract = new LFigure(strToDraw, stepSize, angle)
                        {
                            Stroke = Brushes.Black,
                            StrokeThickness = 1
                        };
            DrawCanvas.Children.Add(fract);

            fract.Loaded += (o, args) =>
                            {
                                Canvas.SetLeft(fract, 10);
                                Canvas.SetTop(fract, Math.Abs(fract.FigureBound.Top) + 10);
                                DrawCanvas.Width = fract.FigureBound.Width;
                                DrawCanvas.Height = fract.FigureBound.Height + 30;
                            };
        }
    }
}
