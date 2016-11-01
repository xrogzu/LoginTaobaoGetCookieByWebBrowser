using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace LoginTaobaoGetCookieByWebBrowser
{
    public partial class MainWindow : Form
    {
        private string TaobaoCookie;
        public MainWindow()
        {
            InitializeComponent();
        }

        // 获取当前WebBrowser登录后的Cookie字符串
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchUrl, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        /// <summary>
        /// 获取某 URL 的 Cookie 返回字符串
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private static string GetCookieString(string Url)
        {
            uint Datasize = 1024;
            StringBuilder CookieData = new StringBuilder((int)Datasize);
            if (!InternetGetCookieEx(Url, null, CookieData, ref Datasize, 0x2000, IntPtr.Zero))
            {
                if (Datasize < 0)
                    return null;
                CookieData = new StringBuilder((int)Datasize);
                if (!InternetGetCookieEx(Url, null, CookieData, ref Datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return CookieData.ToString();
        }
        private void LoginPageWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // e.Url 是当前加载的页面 URL
            if (e.Url.ToString().Contains("https://login.taobao.com/member/login.jhtml"))
            {
                // 自动填充
                try { this.LoginPageWebBrowser.Document.GetElementById("TPL_username_1").SetAttribute("value", "哈哈"); } catch { }
            }
            // 如果跳转到了 URL包含 https://www.taobao.com 的地方
            else if (e.Url.ToString().Contains("https://www.taobao.com"))
            {
                string Cookie = GetCookieString(e.Url.ToString());
                MessageBox.Show(Cookie, "Cookie获取成功", MessageBoxButtons.OK, MessageBoxIcon.None);
                // 然后你可以执行其他操作...
                // 例如：
                this.TaobaoCookie = Cookie;
                // 假如这个窗口有爸爸 →_→
                // this.Close(); // 必须随即关闭此窗口停止执行啦
            }
            // 否则...
            else
            {
                // MessageBox.Show("登录失败？！", "Cookie获取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

}
