using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMSample003.Models
{
    /// <summary>
    /// 時間がかかる処理をするクラス
    /// </summary>
    class HeavyWork
    {
        /// <summary>
        /// 非同期処理のCancellationTokenSource
        /// </summary>
        private CancellationTokenSource _cancelTokenSrc;

        /// <summary>
        /// データ件数
        /// </summary>
        private static readonly int s_dataCount = 100;

        /// <summary>
        /// 時間のかかる処理を実行
        /// </summary>
        /// <param name="progress"></param>
        /// <returns></returns>
        public async Task ExecuteAsync(IProgress<ProgressInfo> progress)
        {
            using (_cancelTokenSrc = new CancellationTokenSource())
            {
                try
                {
                    // 何か時間のかかる処理
                    for (int i = 0; i < s_dataCount; i++)
                    {
                        // キャンセルされたら例外発生
                        _cancelTokenSrc.Token.ThrowIfCancellationRequested();

                        await Task.Delay(50);

                        // 進捗状況通知
                        progress.Report(new ProgressInfo(i + 1, $"{i + 1} / {s_dataCount} 件処理"));
                    }

                    await Task.Delay(1000);

                    progress.Report(new ProgressInfo(0, "処理完了！"));
                }
                catch (OperationCanceledException)
                {
                    progress.Report(new ProgressInfo(0, "処理をキャンセルしました。"));
                    return;
                }
            }
        }


        /// <summary>
        /// 処理キャンセル
        /// </summary>
        public void Cancel()
        {
            if (_cancelTokenSrc?.IsCancellationRequested == false)
            {
                _cancelTokenSrc.Cancel();
            }
        }
    }
}
