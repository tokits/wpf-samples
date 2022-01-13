using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;


/// <summary>
/// ScottPlotのサンプル
/// 参考サイト
/// https://scottplot.net/faq/live-data/
/// </summary>
namespace Graph01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly double[] Values = new double[25];
        readonly Stopwatch Stopwatch = Stopwatch.StartNew();

        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);


        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            UpdateValues();
            wpfPlot.Plot.AddSignal(Values);

            timer.Interval = new TimeSpan(0, 0, 0, 0, 100);

            timer.Tick += (e, s) => {
                UpdateValues();
                wpfPlot.Render();
                wpfPlot.InvalidateVisual();
            };
            this.Closing += (e, s) => { timer.Stop(); };

            timer.Start();
        }

        public void UpdateValues()
        {
            double phase = Stopwatch.Elapsed.TotalSeconds;
            double multiplier = 2 * Math.PI / Values.Length;
            for (int i = 0; i < Values.Length; i++)
                Values[i] = Math.Sin(i * multiplier + phase);
        }
    }
}
