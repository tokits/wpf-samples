using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample005.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        private string input;

        /// <summary>
        /// 入力値
        /// </summary>
        public string Input
        {
            get => input;
            set {
                SetProperty(ref input, value);
                ConvertCommand?.NotifyCanExecuteChanged();
            }
        }

        private string output;

        public string Output
        {
            get => output;
            set => SetProperty(ref output, value);
        }

        public IRelayCommand ConvertCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            ConvertCommand = new RelayCommand(
                execute: ConvertExecute,
                canExecute: CanConvertExecute
                );
        }

        /// <summary>
        /// 大文字に変換
        /// </summary>
        private void ConvertExecute()
        {
            Output = Input.ToUpper();
        }

        /// <summary>
        /// 何か入力されてたら実行可能
        /// </summary>
        /// <returns></returns>
        private bool CanConvertExecute()
        {
            return !string.IsNullOrWhiteSpace(Input);
        }
    }
}
