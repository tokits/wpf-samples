using System;
using System.Collections.Generic;
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
namespace Graph03
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        readonly double[] Values = new double[100_000];
        readonly ScottPlot.Plottable.SignalPlot SignalPlot;
        int NextPointIndex = 0;

        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer1 = new DispatcherTimer(DispatcherPriority.Render);
        private DispatcherTimer timer2 = new DispatcherTimer(DispatcherPriority.Render);

        public MainWindow()
        {
            InitializeComponent();
        
            SignalPlot = wpfPlot.Plot.AddSignal(Values);
            wpfPlot.Plot.SetAxisLimits(0, 100, -2, 2);
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            timer1.Interval = new TimeSpan(0, 0, 0, 0, 10);
            timer1.Tick += (e, s) => {
                Values[NextPointIndex] = Math.Sin(NextPointIndex * .05);
                SignalPlot.MaxRenderIndex = NextPointIndex;
                NextPointIndex += 1;
            };

            timer2.Interval = new TimeSpan(0, 0, 0, 0, 1000);
            timer2.Tick += (e, s) =>
            {
                // adjust the axis limits only when needed
                double currentRightEdge = wpfPlot.Plot.GetAxisLimits().XMax;
                if (NextPointIndex > currentRightEdge)
                    wpfPlot.Plot.SetAxisLimits(xMax: currentRightEdge + 100);

                wpfPlot.Render();
            };

            this.Closing += (e, s) => {
                timer1.Stop();
                timer2.Stop();
            };

            timer1.Start();
            timer2.Start();
            wpfPlot.Render();

        }
    }
}
