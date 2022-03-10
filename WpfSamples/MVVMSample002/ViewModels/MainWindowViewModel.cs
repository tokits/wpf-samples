using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

/// <summary>
/// 参考サイト
/// WPF MVVMサンプル（処理中表示）
/// https://zenn.dev/apterygiformes/articles/026d501aa6fef8
/// </summary>
namespace MVVMSample002.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        private bool _isBusy;
        /// <summary>
        /// 処理中の時true
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private string _statusMessage;
        /// <summary>
        /// ステータス メッセージ
        /// </summary>
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        /// <summary>
        /// 実行コマンド
        /// </summary>
        public ICommand ExecCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            IsBusy = false;
            StatusMessage = "";
            ExecCommand = new AsyncRelayCommand(ExecAsync);
        }

        /// <summary>
        /// 時間のかかる処理
        /// </summary>
        /// <returns></returns>
        private async Task ExecAsync()
        {
            IsBusy = true;
            StatusMessage = "処理中...";

            // 時間のかかる処理
            await Task.Delay(5000);

            IsBusy = false;
            StatusMessage = "処理完了!";
        }
    }
}
