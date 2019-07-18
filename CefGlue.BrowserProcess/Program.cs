using Xilium.CefGlue.Common;

namespace Xilium.CefGlue.BrowserProcess
{
    class Program
    {
        static void Main(string[] args)
        {
            var cefApp = new CommonCefApp(args);
            
            cefApp.RunBrowserProcess();
        }
    }
}