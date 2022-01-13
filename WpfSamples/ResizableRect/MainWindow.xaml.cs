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

namespace ResizableRect
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }


        bool movePinned = false;
        Point? movePiinedPoint = null;
        bool resizePinned1 = false;
        Point? resizePiinedPoint1 = null;
        bool resizePinned2 = false;

        private bool IsInclude1(Point p, double x, double y, double width, double height, int margin)
        {
            bool ret = false;

            if (((x + margin) <= p.X && p.X <= (x + width - margin))
                && ((y + margin) <= p.Y && p.Y <= (y + height - margin))) ret = true;

            return ret;
        }

        private bool IsInclude2(Point p, double x, double y, double width, double height, int margin)
        {
            bool ret = false;

            if (((x + width - margin) <= p.X && p.X <= (x + width))
                || ((y + height - margin) <= p.Y && p.Y <= (y + height))) ret = true;

            return ret;
        }

        private bool IsInclude3(Point p, double x, double y, double width, double height, int margin)
        {
            bool ret = false;

            if ((x <= p.X && p.X <= (x + margin))
                || (y <= p.Y && p.Y <= (y + margin))) ret = true;

            return ret;
        }

        private void Rectangle_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Debug.WriteLine(string.Format("{0},{1}", this.rect.Margin.Left, this.rect.Margin.Top));
            //Debug.WriteLine(e.GetPosition(this));

            //Rectangle r = new Rectangle();
            var p = e.GetPosition(this);
            
            movePinned = IsInclude1(p, rect.Margin.Left, rect.Margin.Top, rect.Width, rect.Height, 10);
            //movePinned = true;
            if (!movePinned)
            {
                resizePinned1 = IsInclude2(p, rect.Margin.Left, rect.Margin.Top, rect.Width, rect.Height, 10);
                if (!resizePinned1)
                {
                    resizePinned2 = IsInclude3(p, rect.Margin.Left, rect.Margin.Top, rect.Width, rect.Height, 10);
                }
            }
            //resizePinned1 = !movePinned;

            if (movePinned)
            {
                Debug.WriteLine(e.GetPosition(rect));
                movePiinedPoint = e.GetPosition(rect);
            }
            if (resizePinned1 || resizePinned2)
            {
                resizePiinedPoint1 = p;
                Debug.WriteLine(resizePiinedPoint1);
            }
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            movePinned = false;
            resizePinned1 = false;
            resizePinned2 = false;
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            if (movePinned)
            {
                var p = e.GetPosition(this);
                //Debug.WriteLine(e.GetPosition(this));
                var x = p.X - movePiinedPoint.Value.X;
                var y = p.Y - movePiinedPoint.Value.Y;
                //Debug.WriteLine($"{x} {y}");
                Debug.WriteLine($"{p.X} {movePiinedPoint.Value.X}");
                rect.Margin = new Thickness(x, y, 0, 0);
                //canvas.Width = canvas.Width + x;
                //canvas.Height = canvas.Height + y;
                //rect_piined_point_e = p;
                //movePiined_point = e.GetPosition(rect);
            }
            if (resizePinned1)
            {
                var p = e.GetPosition(this);
                //Debug.WriteLine(e.GetPosition(this));
                var x = p.X - resizePiinedPoint1.Value.X;
                var y = p.Y - resizePiinedPoint1.Value.Y;
                //Debug.WriteLine($"{x} {y}");
                Debug.WriteLine($"{p.X} {resizePiinedPoint1.Value.X}");
                //canvas.Margin = new Thickness(x, y, 0, 0);
                rect.Width = rect.Width + x;
                rect.Height = rect.Height + y;
                resizePiinedPoint1 = p;

            }
            if (resizePinned2)
            {
                var p = e.GetPosition(this);
                //Debug.WriteLine(e.GetPosition(this));
                var x = p.X - resizePiinedPoint1.Value.X;
                var y = p.Y - resizePiinedPoint1.Value.Y;
                //Debug.WriteLine($"{x} {y}");
                Debug.WriteLine($"{p.X} {resizePiinedPoint1.Value.X}");
                //canvas.Margin = new Thickness(x, y, 0, 0);
                rect.Width = rect.Width - x;
                rect.Height = rect.Height - y;
                rect.Margin = new Thickness(rect.Margin.Left + x, rect.Margin.Top + y, 0, 0);
                resizePiinedPoint1 = p;

            }
        }

        private void Rectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            movePinned = false;
            resizePinned1 = false;
            resizePinned2 = false;
        }
    }
}
