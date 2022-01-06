using System;
using System.Threading;
using System.Threading.Tasks;

/// <summary>
/// 下記サイトの動作確認
/// https://tech-lab.sios.jp/archives/15711
/// </summary>
namespace AsyncAwait01
{
    class Program
    {
        static void Main(string[] args)
        {
            //Program01.Start();
            //Program02.Start();
            Program03.Start();
        }
    }

    /// <summary>
    /// 同期処理
    /// </summary>
    class Program01
    {
        public static void Start()
        {
            string result = HeavyMethod1();

            HeavyMethod2();

            Console.WriteLine(result);

            Console.ReadLine();
        }

        static string HeavyMethod1()
        {
            Console.WriteLine("すごく重い処理その1 Start");
            Thread.Sleep(5000);
            Console.WriteLine("すごく重い処理その1 End");
            return "hoge";
        }

        static void HeavyMethod2()
        {
            Console.WriteLine("すごく重い処理その2 Start");
            Thread.Sleep(3000);
            Console.WriteLine("すごく重い処理その2 End");
        }
    }

    /// <summary>
    /// Threadで非同期処理
    /// </summary>
    class Program02
    {
        public static void Start()
        {
            // メインスレッドからアクセスできる変数を定義します。これに結果を格納します。
            string result = "";
            Thread thread = new Thread(new ThreadStart(() =>
            {
                // メインスレッドとは異なるスレットでHeavyMethodを実行して、
                // 結果(hoge)をresultに格納します。
                result = HeavyMethod1();
            }));

            // スレッドを開始します。
            thread.Start();

            // HeavyMethod2を実行します。
            HeavyMethod2();

            // Joinメソッドを使うとスレッドが終了するまで、これより先のコードに
            // 進まないようになります。
            thread.Join();

            // 結果をコンソールに表示します。
            Console.WriteLine(result);

            Console.ReadLine();
        }

        static String HeavyMethod1()
        {
            Console.WriteLine("すごく重い処理その1 Start");
            Thread.Sleep(5000);
            Console.WriteLine("すごく重い処理その1 End");
            return "hoge";
        }

        static void HeavyMethod2()
        {
            Console.WriteLine("すごく重い処理その2 Start");
            Thread.Sleep(3000);
            Console.WriteLine("すごく重い処理その2 End");
        }
    }

    /// <summary>
    /// async/awaitで非同期処理
    /// </summary>
    class Program03
    {
        public static void Start()
        {
            Task<string> task = HeavyMethod1();

            HeavyMethod2();

            Console.WriteLine(task.Result);

            Console.ReadLine();
        }

        static async Task<string> HeavyMethod1()
        {
            Console.WriteLine("すごく重い処理その1 Start");
            await Task.Delay(5000);
            Console.WriteLine("すごく重い処理その1 End");
            return "hoge";
        }

        static void HeavyMethod2()
        {
            Console.WriteLine("すごく重い処理その2 Start");
            Thread.Sleep(3000);
            Console.WriteLine("すごく重い処理その2 End");
        }
    }
}
