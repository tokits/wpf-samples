using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample003.Models
{
    /// <summary>
    /// 進捗情報
    /// </summary>
    class ProgressInfo
    {
        /// <summary>
        /// 進捗値
        /// </summary>
        public int ProgressValue { get; set; }

        /// <summary>
        /// 進捗テキスト
        /// </summary>
        public string ProgressText { get; set; }

        public ProgressInfo(int value, string text)
        {
            ProgressValue = value;
            ProgressText = text;
        }
    }
}
