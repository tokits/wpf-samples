using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample006.Models
{
    class Calc : ObservableObject
    {
        private double lhs;

        public double Lhs
        {
            get => lhs;
            set => SetProperty(ref lhs, value);
        }

        private double rhs;

        public double Rhs
        {
            get => rhs;
            set => SetProperty(ref rhs, value);
        }

        private OperatorType operatorType;

        public OperatorType OperatorType
        {
            get => operatorType;
            set => SetProperty(ref operatorType, value);
        }

        private double answer;

        public double Answer
        {
            get => answer;
            set => SetProperty(ref answer, value);
        }

        private AppContext appContext;

        public Calc(AppContext appContext)
        {
            this.appContext = appContext;
        }

        public void Execute()
        {
            switch (OperatorType)
            {
                case OperatorType.Add:
                    Answer = Lhs + Rhs;
                    break;
                case OperatorType.Sub:
                    Answer = Lhs - Rhs;
                    break;
                case OperatorType.Mul:
                    Answer = Lhs * Rhs;
                    break;
                case OperatorType.Div:
                    if (Rhs == 0)
                    {
                        appContext.Message = "0除算エラー";
                        return;
                    }
                    Answer = Lhs / Rhs;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }

    public enum OperatorType
    {
        Add,
        Sub,
        Mul,
        Div,
    }
}
