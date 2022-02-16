using ScottPlot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Graph04
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //Random rand = new Random();
        double[] liveData = new double[400];
        DataGen.Electrocardiogram ecg = new DataGen.Electrocardiogram();
        Stopwatch sw = Stopwatch.StartNew();

        private Timer _updateDataTimer;
        private DispatcherTimer _renderTimer;


        public MainWindow()
        {
            InitializeComponent();
 
            // plot the data array only once
            wpfPlot.Plot.AddSignal(liveData);
            wpfPlot.Plot.AxisAutoX(margin: 0);
            wpfPlot.Plot.SetAxisLimits(yMin: -1, yMax: 2.5);
            wpfPlot.Refresh();

            // create a traditional timer to update the data
            _updateDataTimer = new Timer(_ => UpdateData(), null, 0, 5);

            // create a separate timer to update the GUI
            _renderTimer = new DispatcherTimer();
            _renderTimer.Interval = TimeSpan.FromMilliseconds(10);
            _renderTimer.Tick += Render;
            _renderTimer.Start();

            Closed += (sender, args) =>
            {
                _updateDataTimer?.Dispose();
                _renderTimer?.Stop();
            };
        }

        void UpdateData()
        {
            // "scroll" the whole chart to the left
            Array.Copy(liveData, 1, liveData, 0, liveData.Length - 1);

            // place the newest data point at the end
            double nextValue = ecg.GetVoltage(sw.Elapsed.TotalSeconds);
            liveData[liveData.Length - 1] = nextValue;
        }

        void Render(object sender, EventArgs e)
        {
            wpfPlot.Refresh();
        }
    }
}
