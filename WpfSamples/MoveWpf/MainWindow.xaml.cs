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

namespace MoveWpf
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        System.Timers.Timer timer = new System.Timers.Timer();
        
        public MainWindow()
        {
            InitializeComponent();
            //timer.
        }

        bool pinned = false;
        Point ? rect_piined_point = null;

        private bool IsInclude(Point p, double x, double y, double width, double height)
        {
            bool ret = false;

            if ((x <= p.X && p.X <= (x + width))
                && (y <= p.Y && p.Y <= (y + height))) ret = true;

            return ret;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            //Debug.WriteLine(string.Format("{0},{1}", this.rect.Margin.Left, this.rect.Margin.Top));
            //Debug.WriteLine(e.GetPosition(this));

            //Rectangle r = new Rectangle();
            var p = e.GetPosition(this);

            pinned = IsInclude(p, rect.Margin.Left, rect.Margin.Top, rect.Width, rect.Height);

            Debug.WriteLine(e.GetPosition(rect));
            rect_piined_point = e.GetPosition(rect);
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            pinned = false;
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (pinned)
            {
                var p = e.GetPosition(this);
                //Debug.WriteLine(e.GetPosition(this));
                rect.Margin = new Thickness(p.X - rect_piined_point.Value.X, p.Y - rect_piined_point.Value.Y, 0, 0);
            }
        }

        private void Grid_MouseLeave(object sender, MouseEventArgs e)
        {
            pinned = false;
        }
    }
}
