using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using MVVMSample004.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMSample004.ViewModels
{
    class MainWindowViewModel : ObservableObject
    {
        private PageViewModelBase _currentPage;
        /// <summary>
        /// 現在ページ
        /// </summary>
        public PageViewModelBase CurrentPage
        {
            get => _currentPage;
            set
            {
                SetProperty(ref _currentPage, value);
                // ページ戻る/進むコマンドの有効/無効状態更新
                PageBackCommand?.NotifyCanExecuteChanged();
                PageNextCommand?.NotifyCanExecuteChanged();
            }
        }


        /// <summary>
        /// 最初のページVM
        /// </summary>
        public StartPageViewModel StartPage { get; }

        /// <summary>
        /// 設定ページ1のVM
        /// </summary>
        public Setting1PageViewModel Setting1Page { get; }

        /// <summary>
        /// 設定ページ2のVM
        /// </summary>
        public Setting2PageViewModel Setting2Page { get; }

        /// <summary>
        /// 最後のページVM
        /// </summary>
        public EndPageViewModel EndPage { get; }

        /// <summary>
        /// ページ管理リスト
        /// </summary>
        private List<PageViewModelBase> _pages;

        /// <summary>
        /// ページ戻るコマンド
        /// </summary>
        public IRelayCommand PageBackCommand { get; }

        /// <summary>
        /// ページ進むコマンド
        /// </summary>
        public IRelayCommand PageNextCommand { get; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindowViewModel()
        {
            // ページVM初期化（ページの並び順と同じ順番で追加すること）
            _pages = new List<PageViewModelBase>();
            _pages.Add(StartPage = new StartPageViewModel());
            _pages.Add(Setting1Page = new Setting1PageViewModel());
            _pages.Add(Setting2Page = new Setting2PageViewModel());
            _pages.Add(EndPage = new EndPageViewModel());

            // 現在ページ設定
            CurrentPage = StartPage;

            // ページ戻る/進むコマンド初期化
            PageBackCommand = new RelayCommand(PageBackExecute, PageBackCanExecute);
            PageNextCommand = new RelayCommand(PageNextExecute, PageNextCanExecute);
        }

        /// <summary>
        /// ページ戻るコマンドの処理
        /// </summary>
        private void PageBackExecute()
        {
            // ページ管理リストを使用して、現在ページの1つ前を現在ページに設定
            CurrentPage = _pages[_pages.FindIndex(x => x == CurrentPage) - 1];
        }

        /// <summary>
        /// ページ戻るコマンド実行可否を設定
        /// </summary>
        /// <returns></returns>
        private bool PageBackCanExecute()
        {
            // 現在ページが最初のページ以外ならコマンド実行可
            return CurrentPage != _pages.First();
        }

        /// <summary>
        /// ページ進むコマンドの処理
        /// </summary>
        private void PageNextExecute()
        {
            // ページ管理リストを使用して、現在ページの1つ次を現在ページに設定
            CurrentPage = _pages[_pages.FindIndex(x => x == CurrentPage) + 1];

            // 現在ページが最終ページでなければここで終了
            if (CurrentPage != _pages.Last())
            {
                return;
            }

            // ここから最終ページ用の処理

            var sb = new StringBuilder();
            // 全設定ページの設定値を収集
            foreach (var p in _pages)
            {
                // 設定情報取得
                var settings = p.GetSettingStrings();
                if (!settings.Any())
                {
                    // 設定なし
                    continue;
                }

                // ページタイトル
                sb.AppendLine($"■{p.PageTitle}");
                // ページの設定項目
                sb.AppendLine(string.Join(
                    Environment.NewLine,
                    settings.Select(x => $"　* {x}")));
            }

            // 最終ページに設定値一覧を設定
            EndPage.SettingListText = sb.ToString();
        }

        /// <summary>
        /// ページ進むコマンド実行可否を設定
        /// </summary>
        /// <returns></returns>
        private bool PageNextCanExecute()
        {
            // 現在ページが最終ページ以外ならコマンド実行可
            return CurrentPage != _pages.Last();
        }
    }
}
