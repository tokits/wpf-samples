using MVVMSample001.ViewModels;
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
using System.Windows.Shapes;

/// <summary>
/// 参考サイト
/// WPF MVVMサンプル (足し算)
/// https://zenn.dev/apterygiformes/articles/79a7c9e7e15106
/// </summary>
namespace MVVMSample001.Views
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // xamlで指定できないのか？
            // https://www.curict.com/item/51/51fd5f6.html
            // https://garafu.blogspot.com/2014/09/wpf-datacontext.html
            //DataContext = new MainWindowViewModel();
        }
    }
}
