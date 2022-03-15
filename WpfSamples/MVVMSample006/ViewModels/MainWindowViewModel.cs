using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMSample006.Models;
using System.ComponentModel;
using System.Diagnostics;
using System.Reflection;

namespace MVVMSample006.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        private string lhs;

        public string Lhs
        {
            get => lhs; 
            set
            {
                SetProperty(ref lhs, value);
                ExecuteCommand?.NotifyCanExecuteChanged();
            }
        }

        private string rhs;

        public string Rhs
        {
            get => rhs;
            set
            {
                SetProperty(ref rhs, value);
                ExecuteCommand?.NotifyCanExecuteChanged();
            }
        }

        private double answer;

        public double Answer
        {
            get =>answer;
            set => SetProperty(ref answer, value);
        }

        private string message;

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        /// <summary>
        /// 実行コマンド
        /// </summary>
        public IRelayCommand ExecuteCommand { get; }

        public OperatorTypeViewModel[] OperatorTypes { get; private set; }

        private OperatorTypeViewModel selectedOperatorType;

        public OperatorTypeViewModel SelectedOperatorType
        {
            get => selectedOperatorType;
            set
            {
                SetProperty(ref selectedOperatorType, value);
                ExecuteCommand?.NotifyCanExecuteChanged();
            }
        }

        private MVVMSample006.Models.AppContext appContext = new MVVMSample006.Models.AppContext();

        public MainWindowViewModel()
        {
            OperatorTypes = OperatorTypeViewModel.OperatorTypes;

            ExecuteCommand = new RelayCommand(Execute, CanExecute);

            // Modelの監視
            appContext.PropertyChanged += AppContextPropertyChanged;
            appContext.Calc.PropertyChanged += CalcPropertyChanged;
        }

        private void CalcPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Answer")
            {
                Answer = appContext.Calc.Answer;
            }
        }

        private void AppContextPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Message")
            {
                Message = appContext.Message;
            }
        }

        private void Execute()
        {
            appContext.Calc.Lhs = double.Parse(Lhs);
            appContext.Calc.Rhs = double.Parse(Rhs);
            appContext.Calc.OperatorType = SelectedOperatorType.OperatorType;
            appContext.Calc.Execute();
        }

        private bool CanExecute()
        {
            Debug.WriteLine($"{MethodBase.GetCurrentMethod().Name}()");

            double dummy;
            if (!double.TryParse(Lhs, out dummy))
            {
                return false;
            }

            if (!double.TryParse(Rhs, out dummy))
            {
                return false;
            }

            if (this.SelectedOperatorType == null)
            {
                return false;
            }

            return true;
        }
    }
}
