using ScottPlot;
using ScottPlot.Plottable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PlotTest
{


    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    /// 
    public partial class MainWindow : Window
    {


        byte[] sendBuf;          // 送信バッファ   
        int sendByteLen;         //　送信データのバイト数

        byte[] rcvBuf;           // 受信バッファ
        int srcv_pt;             // 受信データ格納位置

        int data_receive_thread_id;  // データ受信ハンドラのスレッドID
        int data_receive_thread_cnt;  // データ受信ハンドラの実施回数

        DispatcherTimer SendIntervalTimer; // タイマー　モニタ用　電文送信間隔   
        DispatcherTimer RcvWaitTimer;       // タイマー　受信待ち用 

        UInt32 msg_num;         // メッセージ番号

        // リアルタイム PV,MVグラフ用 
        uint trend_data_item_max;             // 各リアルタイム　トレンドデータの保持数(=10 ) 2秒毎に収集すると、20秒分のデータ

        double[] trend_data0;                 // トレンドデータ 0   PV(ch1)
        double[] trend_data1;                 // トレンドデータ 1   SV

        double[] trend_dt;                    // トレンドデータ　収集日時

        byte[] send_data0;
        byte[] send_data1;


        ScottPlot.Plottable.ScatterPlot trend_signal_0; // トレンドデータ0 
        ScottPlot.Plottable.ScatterPlot trend_signal_1; // トレンドデータ1

        DateTime receiveDateTime;           // 受信完了日時
        
        private Timer timer2;
        private Random random = new();

        bool init = false;

        //ScatterPlot plot;

        public MainWindow()
        {
            InitializeComponent();

            ConfSerial.serialPort = new SerialPort();    // シリアルポートのインスタンス生成
            ConfSerial.serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);  // データ受信時のイベント処理


            sendBuf = new byte[2048];     // 送信バッファ領域  serialPortのWriteBufferSize =2048 byte(デフォルト)
            rcvBuf = new byte[4096];      // 受信バッファ領域   SerialPort.ReadBufferSize = 4096 byte (デフォルト

            SendIntervalTimer = new System.Windows.Threading.DispatcherTimer();　　// タイマーの生成
            SendIntervalTimer.Tick += new EventHandler(SendIntervalTimer_Tick);  // タイマーイベント
            SendIntervalTimer.Interval = new TimeSpan(0, 0, 0, 0, 1000);         // タイマーイベント発生間隔 1sec（送信間隔)

            RcvWaitTimer = new System.Windows.Threading.DispatcherTimer();　 // タイマーの生成(受信待ちタイマ)
            RcvWaitTimer.Tick += new EventHandler(RcvWaitTimer_Tick);        // タイマーイベント
            RcvWaitTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);          // タイマーイベント発生間隔 (受信待ち時間)

            trend_data_item_max = 10;             // 各リアルタイム　トレンドデータの保持数(=10 )

            Chart_Ini();                        // チャート(リアルタイム用)の初期化


            send_data0 = new byte[trend_data_item_max];
            send_data1 = new byte[trend_data_item_max];

            for (int i = 0; i < trend_data_item_max; i++)               // 送信データの作成
            {
                send_data0[i] = (byte)i;
                send_data1[i] = (byte)trend_data_item_max;
            }

            //DataReceivedHandler2();
            //DataReceivedHandler2();


            //DataReceivedHandler2();
            //DelayProc(DataReceivedHandler2, 100);

            //timer2 = new Timer(_ => DataReceivedHandler2(), null, 0, 1000);
            DelayProc(() => { timer2 = new Timer(_ => DataReceivedHandler2(), null, 0, 1000); }, 2000);
        }


        public static void DelayProc(Action proc, int milliseconds)
        {
            var t = new DispatcherTimer(DispatcherPriority.Render);
            t.Interval = new TimeSpan(0, 0, 0, 0, milliseconds);
            t.Tick += (ts, te) =>
            {
                ((DispatcherTimer)ts).Stop();
                proc();
            };
            t.Start();
        }

        // 　チャートの初期化(リアルタイム　チャート用)
        //    
        private void Chart_Ini()
        {

            trend_data0 = new double[trend_data_item_max];
            trend_data1 = new double[trend_data_item_max];

            trend_dt = new double[trend_data_item_max];


            DateTime datetime = DateTime.Now;   // 現在の日時

            DateTime[] myDates = new DateTime[trend_data_item_max];

            for (int i = 0; i < trend_data_item_max; i++)
            {
                //trend_data0[i] = i;
                //trend_data1[i] = trend_data_item_max;
                trend_data0[i] = 0;
                trend_data1[i] = 0;

                myDates[i] = datetime + new TimeSpan(0, 0, i);  // i秒増やす

                //trend_dt[i] = myDates[i].ToOADate();   // (現在の日時 + i 秒)をdouble型に変換
            }
            //trend_dt[0] = datetime.ToOADate();


            //wpfPlot_PV.Refresh();       // データ変更後のリフレッシュ


            trend_signal_0 = wpfPlot_PV.Plot.AddScatter(trend_dt, trend_data0, color: System.Drawing.Color.Orange, label: "PV(ch1)"); // プロット plot the data array only once
            //trend_signal_1 = wpfPlot_PV.Plot.AddScatter(trend_dt, trend_data1, color: System.Drawing.Color.Green, label: "SV");


            // PVグラフ
            wpfPlot_PV.Configuration.Pan = true;               // パン(グラフの移動)可
            wpfPlot_PV.Configuration.ScrollWheelZoom = true;   // ズーム(グラフの拡大、縮小)可

            wpfPlot_PV.Plot.AxisAuto();                         // X軸、Y軸のオートスケール

            wpfPlot_PV.Plot.XAxis.Ticks(true, false, true);         // X軸の大きい目盛り=表示, X軸の小さい目盛り=非表示, X軸の目盛りのラベル=表示
            wpfPlot_PV.Plot.XAxis.TickLabelStyle(fontSize: 14);     //X軸   ラベルのフォントサイズ変更  :
            wpfPlot_PV.Plot.XAxis.TickLabelFormat("HH:mm:ss", dateTimeFormat: true); // X軸　時間の書式(例 12:30:15)、X軸の値は、日時型
            wpfPlot_PV.Plot.XLabel("time");                         // X軸全体のラベル

            wpfPlot_PV.Plot.YAxis.TickLabelStyle(fontSize: 14);     // Y軸   ラベルのフォントサイズ変更  :
            wpfPlot_PV.Plot.YAxis.Label(label: "[℃]", color: System.Drawing.Color.Black);    // Y軸全体のラベル

            wpfPlot_PV.Plot.SetAxisLimits(yMin: 0, yMax: 10);


            var legend1 = wpfPlot_PV.Plot.Legend(enable: true, location: Alignment.UpperRight);   // 凡例の表示

            legend1.FontSize = 14;      // 凡例のフォントサイズ

            wpfPlot_PV.Refresh();       // データ変更後のリフレッシュ
            //wpfPlot_PV.Render();
        }



        //
        //　Start ボタン
        //
        private void Start_Button_Click(object sender, RoutedEventArgs e)
        {

            msg_num = 0;            // メッセージ番号の初期化

            SendIntervalTimer.Start();     // 定周期送信用のタイマー開始

        }

        //
        //　Stop ボタン
        //
        private void Stop_Button_Click(object sender, RoutedEventArgs e)
        {
            SendIntervalTimer.Stop();     //定周期送信用のタイマー停止

            StatusTextBlock.Text = "Stop";
        }



        // 
        // 定周期にデータを送信する。
        //
        private void SendIntervalTimer_Tick(object sender, EventArgs e)
        {
            Send_TestData();     //    テスト用データの 送信

            StatusTextBlock.Text = "";
        }

        //
        //  データの送信
        //
        private void Send_TestData()
        {
            if (ConfSerial.serialPort.IsOpen == true)
            {
                srcv_pt = 0;                   // 受信データ格納位置クリア
                data_receive_thread_cnt = 0;  // 

                sendByteLen = 2;            // 送信バイト数


                sendBuf[0] = send_data0[msg_num];    // 送信データを送信バッファへ格納
                sendBuf[1] = send_data1[msg_num];

                ConfSerial.serialPort.Write(sendBuf, 0, sendByteLen);     // データ送信

                RcvWaitTimer.Start();        // 受信監視タイマー　開始


                if (msg_num < (trend_data_item_max - 1))
                {
                    msg_num = msg_num + 1;        // メッセージ番号インクリメント
                }
                else
                {
                    msg_num = 0;
                }


                SendRcvTextBlock.Text += "Snd: ";         // 送信の意味

                for (int i = 0; i < sendByteLen; i++)   //  送信データの表示
                {
                    if ((i > 0) && (i % 16 == 0))    // 16バイト毎に1行空ける
                    {
                        SendRcvTextBlock.Text += "\r\n";
                    }

                    SendRcvTextBlock.Text += sendBuf[i].ToString("X2") + " ";

                }
                SendRcvTextBlock.Text += "(" + DateTime.Now.ToString("HH:mm:ss.fff") + ")" + "\r\n";   // 時刻

            }

            else
            {
                StatusTextBlock.Text = "Comm port closed !";

            }

        }




        // 送信後、200msec以内に受信文が得られないと、受信エラー
        //  
        private void RcvWaitTimer_Tick(object sender, EventArgs e)
        {
            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;

            RcvWaitTimer.Stop();        // 受信監視タイマー　停止
            SendIntervalTimer.Stop();     //定周期送信用のタイマー停止

            StatusTextBlock.Text = "Receive time out";
        }



        // デリゲート関数の宣言
        private delegate void DelegateFn();

        // データ受信時のイベント処理
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            int rd_num = ConfSerial.serialPort.BytesToRead;       // 受信データ数

            ConfSerial.serialPort.Read(rcvBuf, srcv_pt, rd_num);   // 受信データを読み出して、受信バッファに格納

            srcv_pt = srcv_pt + rd_num;     // 次回の保存位置
            data_receive_thread_cnt++;      // データ受信ハンドラの実施回数のインクリメント

            int id = System.Threading.Thread.CurrentThread.ManagedThreadId;
            data_receive_thread_id = id;                          //データ受信スレッドのID格納


            if (srcv_pt == sendByteLen)  // 最終データ受信済み (受信データ数は、送信バイト数と同一とする)　イベント処理の終了
            {
                RcvWaitTimer.Stop();        // 受信監視タイマー　停止

                receiveDateTime = DateTime.Now;   // 受信完了時刻を得る

                Dispatcher.BeginInvoke(new DelegateFn(RcvProc)); // Delegateを生成して、RcvProcを開始   (表示は別スレッドのため)
            }

        }
        private void DataReceivedHandler2()
        {
            rcvBuf[0] = (byte)random.Next(0, 10);
            rcvBuf[1] = (byte)random.Next(0, 10);

            {
                RcvWaitTimer.Stop();        // 受信監視タイマー　停止

                receiveDateTime = DateTime.Now;   // 受信完了時刻を得る

                if (init == false)
                {
                    for (int i = 0; i < (int)trend_data_item_max; i++)
                    {
                        //var time = receiveDateTime - new TimeSpan(0, 0, 0, 0, (i + 1) * 1000);  // i秒増やす
                        var time = receiveDateTime - new TimeSpan((i + 1) * 10 * 1000 * 1000);  // i秒増やす
                        trend_dt[(int)trend_data_item_max - 1 - i] = time.ToOADate();
                    }
                    init = true;
                }

                Dispatcher.BeginInvoke(new DelegateFn(RcvProc)); // Delegateを生成して、RcvProcを開始   (表示は別スレッドのため)
            }

        }

        //
        // データ受信イベント終了時の処理
        // 受信データの表示
        //
        private void RcvProc()
        {

            // グラフ(リアルタイム)用表示データの作成
            // グラフを左にずらす (一つ前の配列indexへ移動 )
            Array.Copy(trend_data0, 1, trend_data0, 0, trend_data_item_max - 1);
            trend_data0[trend_data_item_max - 1] = (double)rcvBuf[0];           // 受信した最新データを、グラフ用のデータ配列へ格納

            Array.Copy(trend_data1, 1, trend_data1, 0, trend_data_item_max - 1);
            trend_data1[trend_data_item_max - 1] = (double)rcvBuf[1];

            Array.Copy(trend_dt, 1, trend_dt, 0, trend_data_item_max - 1);
            trend_dt[trend_data_item_max - 1] = receiveDateTime.ToOADate();    // 受信日時 double型に変換して、格納//

            //trend_signal_0.OffsetX = trend_dt[0];
            wpfPlot_PV.Plot.AxisAutoX();

            wpfPlot_PV.Refresh();       // データ変更後のリフレッシュ
            wpfPlot_PV.Render();        // リアルタイム グラフの更新

            //wpfPlot_PV.Plot.AxisAuto();   // X軸の範囲を更新

            rcvmsg_disp();                  // 受信データの表示

        }



        // 受信データの表示
        //
        private void rcvmsg_disp()
        {
            string rcv_str = "";

            SendRcvTextBlock.Text += "Rcv: ";      // 受信の意味
            for (int i = 0; i < srcv_pt; i++)   // 表示用の文字列作成
            {
                if ((i > 0) && (i % 16 == 0))    // 16バイト毎に1行空ける
                {
                    rcv_str = rcv_str + "\r\n";
                }

                rcv_str = rcv_str + rcvBuf[i].ToString("X2") + " ";
            }

            SendRcvTextBlock.Text += rcv_str;  // 受信文

            SendRcvTextBlock.Text += "(" + receiveDateTime.ToString("HH:mm:ss.fff") + ")(" + srcv_pt.ToString() + " bytes )" + "\r\n" + "\r\n";

            LogTextScroll.ScrollToBottom();                                // 一番下までスクロール

        }



        // クリアボタンを押した時の処理
        private void Clear_Button_Click(object sender, RoutedEventArgs e)
        {
            msg_num = 0;            // メッセージ番号の初期化

            SendRcvTextBlock.Text = "";

        }

        //　通信ポートの ダイアログを開く
        //  
        private void Serial_Button_Click(object sender, RoutedEventArgs e)
        {
            new ConfSerial().ShowDialog();
        }

        // チェックボックスによる表示
        private void PV_X_Show(object sender, RoutedEventArgs e)
        {

            if (trend_signal_0 is null) return;
            if (trend_signal_1 is null) return;

            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Name == "PV_CheckBox")
            {
                trend_signal_0.IsVisible = true;
            }
            else if (checkBox.Name == "SV_CheckBox")
            {
                trend_signal_1.IsVisible = true;
            }

            wpfPlot_PV.Render();   // グラフの更新

        }


        // チェックボックスによる非表示
        private void PV_X_Hide(object sender, RoutedEventArgs e)
        {

            if (trend_signal_0 is null) return;
            if (trend_signal_1 is null) return;

            CheckBox checkBox = (CheckBox)sender;

            if (checkBox.Name == "PV_CheckBox")
            {
                trend_signal_0.IsVisible = false;
            }
            else if (checkBox.Name == "SV_CheckBox")
            {
                trend_signal_1.IsVisible = false;
            }

            wpfPlot_PV.Render();   // グラフの更新
        }

        // Axise Auto　ボタンを押した時の処理
        // グラフのパン(移動)、ズームで線が見えなくなった時、オートスケールで再表示させる。


        private void Axis_Auto_Button_Click(object sender, RoutedEventArgs e)
        {
            wpfPlot_PV.Plot.AxisAuto();         // X軸、Y軸のオートスケール
            wpfPlot_PV.Render();                // グラフの更新
        }
    }
}