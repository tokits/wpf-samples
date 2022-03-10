using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample004.ViewModels.Pages
{
    class EndPageViewModel : PageViewModelBase
    {
        private string _settingListText;
        /// <summary>
        /// 設定内容一覧テキスト
        /// </summary>
        public string SettingListText
        {
            get => _settingListText;
            set => SetProperty(ref _settingListText, value);
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EndPageViewModel()
        {
            PageTitle = "ページ4";
        }
    }
}
