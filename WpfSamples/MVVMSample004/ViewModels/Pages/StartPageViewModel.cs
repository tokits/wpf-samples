using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample004.ViewModels.Pages
{
    class StartPageViewModel : PageViewModelBase
    {
        private string _message;
        /// <summary>
        /// スタートページに表示するメッセージ
        /// </summary>
        public string Message
        {
            get => _message;
            set => SetProperty(ref _message, value);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public StartPageViewModel()
        {
            PageTitle = "ページ1";
            Message = "最初のページです。";
        }
    }
}
