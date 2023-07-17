using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace NicoTest
{
    static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            try
            {
                var ostype = Util.CheckOSType();
                if (ostype == "XP" || ostype == "Vista")
                {
                    MessageBox.Show("OSはXPかVistaです");
                }
                else if (ostype == "7" || ostype == "8" || ostype == "8.1")
                {
                    MessageBox.Show("OSは7か8か8.1です");
                }
                else if (ostype == "10" || ostype == "11")
                {
                    MessageBox.Show("OSは10か11です");
                }
                else
                {
                    MessageBox.Show("OSはXPより前かUnknownです");
                }
            }
            catch (Exception ex)
            {
                // エラーの場合の例外処理
                MessageBox.Show("Program.csでエラー"+ex.Message.ToString());
            }
            Application.Run(new Form1());
        }
    }
}
