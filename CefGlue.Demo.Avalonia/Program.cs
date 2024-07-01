using Avalonia;
using Xilium.CefGlue.Common;
using Xilium.CefGlue.Common.Shared;

namespace Xilium.CefGlue.Demo.Avalonia
{
    class Program
    {
        static int Main(string[] args)
        {
            return BuildAvaloniaApp()
                .StartWithClassicDesktopLifetime(args);
        }

        /// <summary>
        /// Avalonia configuration, don't remove; also used by visual designer.
        /// </summary>
        /// <returns></returns>
        public static AppBuilder BuildAvaloniaApp() =>
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .With(new Win32PlatformOptions()
                {
                    // CompositionMode = new [] { Win32CompositionMode.WinUIComposition }
                })
                .AfterSetup(_ => CefRuntimeLoader.Initialize(new CefSettings()
                {
#if WINDOWLESS
                    WindowlessRenderingEnabled = true,
#else
                    WindowlessRenderingEnabled = false,
#endif
                }, customSchemes: new[] { new CustomScheme() { SchemeName = "test", SchemeHandlerFactory = new CustomSchemeHandler(), } }));
    }
}
