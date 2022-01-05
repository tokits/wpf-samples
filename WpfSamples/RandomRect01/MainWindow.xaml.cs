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

namespace RandomRect01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// タイマ
        /// </summary>
        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Render);
        /// <summary>
        /// 乱数
        /// </summary>
        Random rondom = new System.Random();

        /// <summary>
        /// 表示する図形オブジェクトの最大個数
        /// </summary>
        public int MaxCount { get; private set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            MaxCount = 100;
        }

        /// <summary>
        /// タイマ初期化
        /// </summary>
        private void InitializeTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Tick += (e, s) => {
                double x = rondom.Next(0, (int)this.grid.ActualWidth);
                double y = rondom.Next(0, (int)this.grid.ActualHeight);
                double width = rondom.Next(0, (int)(this.grid.ActualWidth - x));
                double height = rondom.Next(0, (int)(this.grid.ActualHeight - y));
                Debug.WriteLine($"x={x},y={y},with={width},height={height}");
                Rectangle rect = new Rectangle();
                rect.Width = width;
                rect.Height = height;
                byte r = (byte)rondom.Next(0, 255);
                byte g = (byte)rondom.Next(0, 255);
                byte b = (byte)rondom.Next(0, 255);
                rect.Fill = new SolidColorBrush(Color.FromArgb(100, r, g, b));
                Canvas.SetLeft(rect, x);
                Canvas.SetTop(rect, y);
                canvas.Children.Add(rect);

                // メモリ枯渇しないように上限を決めて、古いオブジェクトは削除する。
                while (canvas.Children.Count > MaxCount)
                {
                    canvas.Children.RemoveAt(0);
                }

            };
            this.Closing += (e, s) => { timer.Stop(); };
        }

        /// <summary>
        /// canvasのInitializedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_Initialized(object sender, EventArgs e)
        {
            InitializeTimer();
            timer.Start();
        }
    }
}
