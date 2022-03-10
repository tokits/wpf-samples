using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample004.ViewModels.Pages
{
    class Setting1PageViewModel : PageViewModelBase
    {
        private bool _check1;
        /// <summary>
        /// ON/OFF項目1
        /// </summary>
        public bool Check1
        {
            get => _check1;
            set => SetProperty(ref _check1, value);
        }

        private bool _check2;
        /// <summary>
        /// ON/OFF項目2
        /// </summary>
        public bool Check2
        {
            get => _check2;
            set => SetProperty(ref _check2, value);
        }

        /// <summary>
        /// 設定文字列取得
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetSettingStrings()
        {
            yield return $"{nameof(Check1)}={Check1}";
            yield return $"{nameof(Check2)}={Check2}";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Setting1PageViewModel()
        {
            PageTitle = "ページ2";
            Check1 = false;
            Check2 = true;
        }
    }
}
