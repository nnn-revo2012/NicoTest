using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Runtime.Serialization;
using System.Net;
using System.IO;
using System.Web;
using System.Diagnostics;

namespace NicoTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            
            var osname = "OS: " + Util.CheckOSName();
            this.textBox1.Text = osname + "\r\n";
            var ostype = "OSType: " + Util.CheckOSType();
            this.textBox1.Text += ostype + "\r\n";
            if (ostype != "XP")
            {
                var ver = Util.Get45PlusFromRegistry();
                this.textBox1.Text += ".NET: " + ver.ToString() + "\r\n";
            }
            if (ostype != "XP" && ostype != "Vista")
            {
                NetTest2();
            }
            else
            {
                this.textBox1.Text += "OSがXPかVistaなのでネット接続テストは行いません\r\n";
            }

        }

        private void NetTest2()
        {
            var strurl = "https://live.nicovideo.jp/";
            string err;
            int neterr;

            try
            {
                ServicePointManager.SecurityProtocol =
                    (SecurityProtocolType)0x00000C00;   //Tls1.2
                //    (SecurityProtocolType)0x00000300;   //Tls1.1

                textBox1.Text += strurl + "にTls1.2で接続します\r\n";
                var url = new Uri(strurl);
                string res;

                using (var wc = new WebClientEx())
                {
                    wc.Encoding = Encoding.UTF8;
                    wc.Headers.Add(HttpRequestHeader.Accept, "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
                    wc.Headers.Add(HttpRequestHeader.AcceptLanguage, "ja,en-US;q=0.9,en;q=0.8");
                    wc.Headers.Add(HttpRequestHeader.UserAgent, "TEST TOOL");
                    wc.Headers.Add(HttpRequestHeader.Referer, "https://www.nicovideo.jp/");
                    wc.Proxy = null;
                    wc.timeout = 30000;
                    res = wc.DownloadString(strurl);
                }
                textBox1.Text += "接続に成功しました" + "\r\n";
            }
            catch (WebException Ex)
            {
                if (Ex.Status == WebExceptionStatus.ProtocolError)
                {
                    HttpWebResponse errres = (HttpWebResponse)Ex.Response;
                    neterr = (int)errres.StatusCode;
                    err = neterr.ToString() + " " + errres.StatusDescription;
                }
                else
                    err = Ex.Message;
                textBox1.Text += "接続に失敗しました" + "\r\n";
                textBox1.Text += "エラー: " + err + "\r\n";
                return;
            }
            catch (Exception Ex) //その他のエラー
            {
                err = Ex.Message;
                textBox1.Text += "接続に失敗しました" + "\r\n";
                textBox1.Text += "エラー: " + err + "\r\n";
                return;
            }
        }
        private class WebClientEx : WebClient
        {
            public CookieContainer cookieContainer = new CookieContainer();
            public int timeout;

            protected override WebRequest GetWebRequest(Uri address)
            {
                var wr = base.GetWebRequest(address);

                HttpWebRequest hwr = wr as HttpWebRequest;
                if (hwr != null)
                {
                    hwr.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate; //圧縮を有効化
                    hwr.CookieContainer = cookieContainer; //Cookie
                    hwr.Timeout = timeout;
                }
                return wr;
            }
        }




    }
}
