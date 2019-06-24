using System;

namespace Xilium.CefGlue.Common
{
    internal sealed class CommonCefDisplayHandler : CefDisplayHandler
    {
        private readonly ICefBrowserHost _owner;

        public CommonCefDisplayHandler(ICefBrowserHost owner)
        {
            if (owner == null) throw new ArgumentNullException("owner");

            _owner = owner;
        }

        protected override void OnAddressChange(CefBrowser browser, CefFrame frame, string url)
        {
        }

        protected override void OnTitleChange(CefBrowser browser, string title)
        {
        }

        protected override bool OnTooltip(CefBrowser browser, string text)
        {
            return _owner.HandleTooltip(text);
        }

        protected override void OnStatusMessage(CefBrowser browser, string value)
        {
        }

        protected override bool OnConsoleMessage(CefBrowser browser, CefLogSeverity level, string message, string source, int line)
        {
            return false;
        }
    }
}