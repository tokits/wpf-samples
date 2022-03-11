using MVVMSample006.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample006.ViewModels
{
    class OperatorTypeViewModel
    {
        public OperatorType OperatorType { get; private set; }
        public string Label { get; private set; }

        public OperatorTypeViewModel(string label, OperatorType operatorType)
        {
            this.Label = label;
            this.OperatorType = operatorType;
        }

        public static OperatorTypeViewModel[] OperatorTypes = new[]
        {
            new OperatorTypeViewModel("足し算", OperatorType.Add),
            new OperatorTypeViewModel("引き算", OperatorType.Sub),
            new OperatorTypeViewModel("掛け算", OperatorType.Mul),
            new OperatorTypeViewModel("割り算", OperatorType.Div),
        };
    }
}
