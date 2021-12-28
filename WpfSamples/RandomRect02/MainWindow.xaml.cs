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

namespace RandomRect02
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
        /// DrawingGroupオブジェクト
        /// </summary>
        DrawingGroup drawingGroup;
        Image image;


        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// タイマ初期化
        /// </summary>
        private void InitializeTimer()
        {
            timer.Interval = new TimeSpan(0, 0, 0, 0, 10);

            timer.Tick += (e, s) => {
                // drawingGroupに対して上書きで矩形を書いていく
                using (DrawingContext drawContent = drawingGroup.Append())
                {
                    double x = rondom.Next(0, (int)canvas.ActualWidth);
                    double y = rondom.Next(0, (int)canvas.ActualHeight);
                    double width = rondom.Next(0, (int)(canvas.ActualWidth - x));
                    double height = rondom.Next(0, (int)(canvas.ActualHeight - y));
                    Debug.WriteLine($"x={x},y={y},with={width},height={height}");
                    var rect = new Rect(x, y, width, height);
                    byte r = (byte)rondom.Next(0, 255);
                    byte g = (byte)rondom.Next(0, 255);
                    byte b = (byte)rondom.Next(0, 255);
                    var brush = new SolidColorBrush(Color.FromArgb(100, r, g, b));
                    drawContent.DrawRectangle(brush, null, rect);
                }

            };
            this.Closing += (e, s) => { timer.Stop(); };
        }

        /// <summary>
        /// canvasのLoadedイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void canvas_Loaded(object sender, RoutedEventArgs e)
        {
            image = new Image();
            drawingGroup = new DrawingGroup();

            // drawingGroupに最大サイズで矩形を書く。
            // drawingGroupの描画領域を最大にしておかないと、後で描画する際ずれて描画する場合があった。
            using (DrawingContext drawContent = drawingGroup.Open())
            {
                double x = 0;
                double y = 0;
                double width = canvas.ActualWidth;
                double height = canvas.ActualHeight;
                Debug.WriteLine($"x={x},y={y},with={width},height={height}");
                var rect = new Rect(x, y, width, height);
                var brush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                drawContent.DrawRectangle(brush, null, rect);
            }
            Canvas.SetLeft(image, 0);
            Canvas.SetTop(image, 0);
            image.Source = new DrawingImage(drawingGroup);
            canvas.Children.Add(image);

            InitializeTimer();
            timer.Start();
        }
    }
}
