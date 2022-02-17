using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace Graph06
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //readonly double[] Values = new double[500];
        //readonly Stopwatch Stopwatch = Stopwatch.StartNew();
        double[] liveData = new double[1000];
        double[] displayData = new double[100];
        readonly ScottPlot.Plottable.VLine VerticalLine;
        int NextIndex = 0;
        int counter = 0;
        int src_index = 0;
        int dst_index = 0;

        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer1;
        private Timer timer2;

        public int PatternIndex { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            PatternIndex = 0;

            wpfPlot.Plot.AddSignal(displayData);
            wpfPlot.Plot.AxisAutoX(margin: 0);
            wpfPlot.Plot.SetAxisLimits(yMin: -1.5, yMax: 1.5);
            VerticalLine = wpfPlot.Plot.AddVerticalLine(0, System.Drawing.Color.Red, 2);
            wpfPlot.Configuration.Pan = false;  // マウスによる移動不可
            wpfPlot.Configuration.Zoom = false; // マウスによるズーム不可

            UpdateData();

            timer1 = new DispatcherTimer(DispatcherPriority.Render);
            timer1.Interval = TimeSpan.FromMilliseconds(100);
            timer1.Tick += Render;
            timer1.Start();
            timer2 = new Timer(_ => UpdateData(), null, 0, 10000);
        }

        void UpdateData()
        {
            for (int i = 0; i < liveData.Length; i++)
            {
                if (PatternIndex == 0)
                {
                    liveData[i] = Math.Sin(Math.PI * (((double)counter) / 20.0));
                }
                else
                {
                    liveData[i] = Math.Sin(Math.PI * (((double)counter) / 20.0)) + 0.1 * Math.Sin(Math.PI * (((double)counter + 50) / 3.0));
                }
                counter++;
                if (counter >= 1000 * 2) counter = 0;
            }
        }

        void Render(object sender, EventArgs e)
        {
            Array.Copy(liveData, src_index, displayData, dst_index, displayData.Length / 100);
            src_index += displayData.Length / 100;
            dst_index += displayData.Length / 100;
            VerticalLine.X = (src_index - 1) % displayData.Length;
            if (src_index >= liveData.Length) src_index = 0;
            if (dst_index >= displayData.Length) dst_index = 0;

            wpfPlot.Refresh();
        }

    }
}
