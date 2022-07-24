using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble.Util
{
    internal class WindowListUtil
    {
        public class WindowInfo
        {
            public IntPtr Handle { get; private set; }

            public string Title { get; private set; }

            public string ClassName { get; private set; }

            public bool IsTopmost { get; set; }

            public WindowInfo(IntPtr handle)
            {
                Handle = handle;
                int length = Win32Util.GetWindowTextLength(Handle) * 2;
                var stringBuilder = new StringBuilder(length);
                _ = Win32Util.GetWindowText(Handle, stringBuilder, length);
                Title = stringBuilder.ToString();
                var className = new StringBuilder(256);
                _ = Win32Util.GetClassName(Handle, className, 256);
                ClassName = className.ToString();
            }
        }

        public static List<WindowInfo> GetWindowList()
        {
            var list = new List<WindowInfo>();
            Win32Util.EnumWindows((hwnd, lParam) =>
            {
                var windowInfo = new WindowInfo(hwnd);
                if (!IsCapturableWindow(windowInfo))
                {
                    return true;
                }
                list.Add(windowInfo);
                return true;
            }, IntPtr.Zero);
            return list;
        }

        private static bool IsKnownBlockedWindow(WindowInfo windowInfo)
        {
            return windowInfo.Title == "Task View" && windowInfo.ClassName == "Windows.UI.Core.CoreWindow" ||
                windowInfo.Title == "DesktopWindowXamlSource" && windowInfo.ClassName == "Windows.UI.Core.CoreWindow" ||
                windowInfo.Title == "PopupHost" && windowInfo.ClassName == "Xaml_WindowedPopupClass";
        }

        private static bool IsCapturableWindow(WindowInfo windowInfo)
        {
            if (string.IsNullOrEmpty(windowInfo.Title) ||
                windowInfo.Handle == Win32Util.GetShellWindow() ||
                !Win32Util.IsWindowVisible(windowInfo.Handle) ||
                Win32Util.GetAncestor(windowInfo.Handle, Win32Util.GA_ROOT) != windowInfo.Handle)
            {
                return false;
            }

            long style = Win32Util.GetWindowLong(windowInfo.Handle, Win32Util.GWL_STYLE);
            if ((style & Win32Util.WS_DISABLED) == Win32Util.WS_DISABLED)
            {
                return false;
            }

            style = Win32Util.GetWindowLong(windowInfo.Handle, Win32Util.GWL_EXSTYLE);
            if ((style & Win32Util.WS_EX_TOOLWINDOW) == Win32Util.WS_EX_TOOLWINDOW)
            {
                return false;
            }

            if (windowInfo.ClassName == "Windows.UI.Core.CoreWindow" || windowInfo.ClassName == "ApplicationFrameWindow")
            {
                if (Win32Util.DwmGetWindowAttribute(windowInfo.Handle, Win32Util.DWMWINDOWATTRIBUTE.DWMWA_CLOAKED, out int cloaked, sizeof(int)) == 0 &&
                    cloaked == Win32Util.DWM_CLOAKED_SHELL)
                {
                    return false;
                }
            }

            if (IsKnownBlockedWindow(windowInfo))
            {
                return false;
            }

            windowInfo.IsTopmost = (style & Win32Util.WS_EX_TOPMOST) == Win32Util.WS_EX_TOPMOST;
            return true;
        }
    }
}
