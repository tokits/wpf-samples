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

namespace Graph05
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double[] liveData = new double[1000];
        double[] displayData = new double[100];

        private Timer _updateDataTimer;
        private DispatcherTimer _renderTimer;

        int counter = 0;
        int copy_index = 0;

        public MainWindow()
        {
            InitializeComponent();

            // plot the data array only once
            wpfPlot.Plot.AddSignal(displayData);
            wpfPlot.Plot.AxisAutoX(margin: 0);
            wpfPlot.Plot.SetAxisLimits(yMin: -1.5, yMax: 1.5);
            wpfPlot.Refresh();

            // create a traditional timer to update the data
            _updateDataTimer = new Timer(_ => UpdateData(), null, 0, 1000);

            // create a separate timer to update the GUI
            _renderTimer = new DispatcherTimer();
            _renderTimer.Interval = TimeSpan.FromMilliseconds(500);
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
            for (int i = 0; i < liveData.Length; i++)
            {
                liveData[i] = Math.Sin(Math.PI * (((double)counter) / 20.0)) + 0.1 * Math.Sin(Math.PI * (((double)counter + 50) / 3.0));
                counter++;
                if (counter >= 1000 * 2) counter = 0;
            }
        }

        void Render(object sender, EventArgs e)
        {
            Array.Copy(liveData, copy_index, displayData, 0, displayData.Length) ;
            copy_index += displayData.Length;
            if (copy_index >= liveData.Length) copy_index = 0;

            wpfPlot.Refresh();
        }
    }
}
