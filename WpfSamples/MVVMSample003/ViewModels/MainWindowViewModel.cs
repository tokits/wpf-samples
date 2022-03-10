using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MVVMSample003.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample003.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        private bool _isBusy;

        /// <summary>
        /// 処理中フラグ
        /// </summary>
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        private int _progressValue;

        /// <summary>
        /// 進捗値
        /// </summary>
        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        private string _progressText;

        /// <summary>
        /// 進捗テキスト
        /// </summary>
        public string ProgressText
        {
            get => _progressText;
            set => SetProperty(ref _progressText, value);
        }

        /// <summary>
        /// 実行コマンド
        /// </summary>
        public IAsyncRelayCommand ExecuteCommand { get; }

        /// <summary>
        /// キャンセルコマンド
        /// </summary>
        public IRelayCommand CancelCommand { get; }

        /// <summary>
        /// モデル
        /// </summary>
        private HeavyWork _model;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            IsBusy = false;

            // 実行コマンド初期化
            ExecuteCommand = new AsyncRelayCommand(OnExecuteAsync, CanExecute);

            // キャンセルコマンド初期化
            CancelCommand = new RelayCommand(
                execute: () =>
                {
                    _model?.Cancel();
                    // コマンドの実行可否 更新
                    UpdateCommandStatus();
                },
                canExecute: () => IsBusy);
        }

        /// <summary>
        /// 実行コマンドの処理
        /// </summary>
        /// <returns></returns>
        private async Task OnExecuteAsync()
        {
            IsBusy = true;
            // コマンドの実行可否 更新
            UpdateCommandStatus();

            _model = new HeavyWork();

            var p = new Progress<ProgressInfo>();
            p.ProgressChanged += (sender, e) =>
            {
                ProgressValue = e.ProgressValue;
                ProgressText = e.ProgressText;
            };

            // 時間のかかる処理 開始
            await _model.ExecuteAsync(p);

            IsBusy = false;
            // コマンドの実行可否 更新
            UpdateCommandStatus();
        }

        /// <summary>
        /// 実行コマンドの実行可否
        /// </summary>
        /// <returns></returns>
        private bool CanExecute()
        {
            // 処理中でなければ実行可
            return !IsBusy;
        }

        /// <summary>
        /// コマンドの実行可否 更新
        /// </summary>
        private void UpdateCommandStatus()
        {
            ExecuteCommand.NotifyCanExecuteChanged();
            CancelCommand.NotifyCanExecuteChanged();
        }
    }
}
