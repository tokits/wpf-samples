using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample004.ViewModels.Pages
{
    class PageViewModelBase : ObservableObject
    {
        private string _pageTitle;
        /// <summary>
        /// ページタイトル
        /// </summary>
        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }

        /// <summary>
        /// 設定文字列取得
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<string> GetSettingStrings()
        {
            return Enumerable.Empty<string>();
        }
    }
}
