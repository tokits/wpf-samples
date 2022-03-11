using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample006.Models
{
    class AppContext : ObservableObject
    {
        private string message;

        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public Calc Calc { get; private set; }

        public AppContext()
        {
            Calc = new Calc(this);
        }
    }
}
