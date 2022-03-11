using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample004.ViewModels.Pages
{
    class Setting2PageViewModel : PageViewModelBase
    {
        private string _text1;
        /// <summary>
        /// テキスト項目1
        /// </summary>
        public string Text1
        {
            get => _text1;
            set => SetProperty(ref _text1, value);
        }

        private string _text2;
        /// <summary>
        /// テキスト項目2
        /// </summary>
        public string Text2
        {
            get => _text2;
            set => SetProperty(ref _text2, value);
        }

        private int _number;

        /// <summary>
        /// 数値
        /// </summary>
        public int Number
        {
            get => _number;
            set => SetProperty(ref _number, value);
        }

        /// <summary>
        /// 設定文字列取得
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetSettingStrings()
        {
            yield return $"{nameof(Text1)}={Text1}";
            yield return $"{nameof(Text2)}={Text2}";
            yield return $"{nameof(Number)}={Number}";
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public Setting2PageViewModel()
        {
            PageTitle = "ページ3";
            Text1 = "こんにちは。";
            Text2 = "";
        }
    }
}
