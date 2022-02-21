using ScottPlot;
using ScottPlot.Plottable;
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
 

namespace Graph07
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double[] displayData = new double[10];
        double[] tickData = new double[10];
        int counter = 0;
        Random random = new();

        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer1;
        private Timer timer2;

        public int PatternIndex { get; set; }

        private ScatterPlot plot;

        private Queue<GraphData> queue = new();

        private bool init = false;
        private bool firstGraph = true;
        private int firstGraphCount = 0;

        public MainWindow()
        {
            InitializeComponent();

            initPlot();

            timer2 = new Timer(_ => UpdateData(), null, 0, 1000);

            //Utilities.DelayProc(() =>
            //{
            //    timer1 = new DispatcherTimer(DispatcherPriority.Render);
            //    //timer1.Interval = TimeSpan.FromMilliseconds((1000 / tickData.Length));
            //    timer1.Interval = TimeSpan.FromMilliseconds(tickData.Length);
            //    timer1.Tick += Render;
            //    timer1.Start();
            //}, 0);
        }

        void initPlot()
        {
            plot = wpfPlot.Plot.AddScatter(tickData, displayData, color: System.Drawing.Color.Orange, label: "Power"); // プロット plot the data array only once


            // PVグラフ
            wpfPlot.Configuration.Pan = false;  // マウスによる移動不可
            wpfPlot.Configuration.Zoom = false; // マウスによるズーム不可

            wpfPlot.Plot.AxisAutoX(margin: 0);
            wpfPlot.Plot.SetAxisLimits(yMin: -1.5, yMax: 1.5);

            wpfPlot.Plot.XAxis.Ticks(true, false, true);         // X軸の大きい目盛り=表示, X軸の小さい目盛り=非表示, X軸の目盛りのラベル=表示
            wpfPlot.Plot.XAxis.TickLabelStyle(fontSize: 14);     //X軸   ラベルのフォントサイズ変更  :
            wpfPlot.Plot.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true); // X軸　時間の書式(例 12:30:15)、X軸の値は、日時型
            wpfPlot.Plot.XLabel("time");                         // X軸全体のラベル

            wpfPlot.Plot.YAxis.TickLabelStyle(fontSize: 14);     // Y軸   ラベルのフォントサイズ変更  :
            wpfPlot.Plot.YAxis.Label(label: "Power", color: System.Drawing.Color.Black);    // Y軸全体のラベル

            var legend1 = wpfPlot.Plot.Legend(enable: true, location: Alignment.UpperRight);   // 凡例の表示

            legend1.FontSize = 14;      // 凡例のフォントサイズ

            wpfPlot.Refresh();       // データ変更後のリフレッシュ
            wpfPlot.Render();
        }

        void UpdateData()
        {
            int max = 1;
            DateTime now = DateTime.Now;
            for (int i = 0; i < max; i++)
            {
                GraphData d = new();
                d.datetime = now + new TimeSpan(0, 0, 0, 0, i * (1000 / max));
                d.dt = d.datetime.ToOADate();
                if (PatternIndex == 0)
                {
                    //d.valule = Math.Sin(Math.PI * (((double)counter) / 2.0));
                    d.valule = 1 - 2 * random.NextDouble();
                }
                else
                {
                    d.valule = Math.Sin(Math.PI * (((double)counter) / 2.0)) + 0.1 * Math.Sin(Math.PI * (((double)counter + 50) / 3.0));
                }

                queue.Enqueue(d);

                counter++;
                if (counter >= 1000 * 2) counter = 0;
            }
            this.Dispatcher.Invoke((Action)(() =>
            {
                Render();
            }));

        }

        void Render()
        {
            if (init == false)
            {
                DateTime first = queue.Peek().datetime;
                for (int i = 0; i < (int)tickData.Length; i++)
                {
                    var time = first - new TimeSpan((i + 1) * 10 * 1000 * 1000);
                    tickData[(int)tickData.Length - 1 - i] = time.ToOADate();
                }
                init = true;
            }

            if (queue.Count > 0)
            {
                Array.Copy(displayData, 1, displayData, 0, displayData.Length - 1);
                Array.Copy(tickData, 1, tickData, 0, tickData.Length - 1);

                int index = displayData.Length - 1;
                var d = queue.Dequeue();
                displayData[index] = d.valule;
                tickData[index] = d.dt;
            }

            //plot.OffsetX = tickData[0];
            wpfPlot.Plot.AxisAutoX();

            wpfPlot.Refresh();       // データ変更後のリフレッシュ
            wpfPlot.Render();        // リアルタイム グラフの更新
        }


    }

    struct GraphData
    {
        public DateTime datetime;
        public double dt;
        public double valule;
    }


}
