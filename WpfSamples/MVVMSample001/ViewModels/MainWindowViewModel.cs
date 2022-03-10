using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 参考サイト
/// WPF MVVMサンプル (足し算)
/// https://zenn.dev/apterygiformes/articles/79a7c9e7e15106
/// </summary>
namespace MVVMSample001.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        /// <summary>
        /// 1つ目の被演算子
        /// </summary>
        private string _value1 = "xxxx";

        /// <summary>
        /// 1つ目の被演算子
        /// </summary>
        public string Value1
        {
            get => _value1;
            set
            {
                SetProperty(ref _value1, value);
                CalculateCommand?.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 2つ目の非演算子
        /// </summary>
        private string _value2;

        /// <summary>
        /// 2つ目の被演算子
        /// </summary>
        public string Value2
        {
            get => _value2;
            set
            {
                SetProperty(ref _value2, value);
                CalculateCommand?.NotifyCanExecuteChanged();
            }
        }

        /// <summary>
        /// 計算結果
        /// </summary>
        private string _result;

        /// <summary>
        /// 計算結果
        /// </summary>
        public string Result
        {
            get => _result;
            set => SetProperty(ref _result, value);
        }

        /// <summary>
        /// 計算コマンド
        /// </summary>
        public IRelayCommand CalculateCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            CalculateCommand = new RelayCommand(
                execute: () =>
                {
                    try
                    {
                        Result = $"{Value1} + {Value2} = {int.Parse(Value1) + int.Parse(Value2)}";
                    }
                    catch (Exception e)
                    {
                        Result = $"{Value1} + {Value2}  Exception Occurred:{e.Message}";
                    }
                },
                canExecute: () =>
                {
                    return !string.IsNullOrEmpty(Value1) && !string.IsNullOrEmpty(Value2);
                }
                );
        }
    }
}
