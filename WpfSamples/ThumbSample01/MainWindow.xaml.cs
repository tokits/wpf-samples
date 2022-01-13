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

namespace ThumbSample01
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// 参考サイト
    /// https://www.moonmile.net/blog/archives/3701
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void printPos(UIElement el)
        {
            int x = (int)Canvas.GetLeft(el);
            int y = (int)Canvas.GetTop(el);
            textPos.Text = string.Format("x:{0} y:{1}", x, y);
        }

        /// <summary>
        /// ドラッグ開始
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragStarted(object sender,
            System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            mark.Background = new SolidColorBrush(Colors.Orange);
        }
        /// <summary>
        /// ドラッグ終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragCompleted(object sender,
            System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            mark.Background = new SolidColorBrush(Colors.Purple);
        }

        /// <summary>
        /// ドラッグ中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mark_DragDelta(object sender,
            System.Windows.Controls.Primitives.DragDeltaEventArgs e)
        {
            printPos(mark);
            Canvas.SetLeft(mark, Canvas.GetLeft(mark) + e.HorizontalChange);
            Canvas.SetTop(mark, Canvas.GetTop(mark) + e.VerticalChange);
        }
    }
}
