using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble.Util
{
    internal class TopmostUtil
    {
        public static bool SetIsTopmost(IntPtr hwnd, bool isTopmost)
        {
            return Win32Util.SetWindowPos(
                hwnd,
                isTopmost ? Win32Util.HWND_TOPMOST : Win32Util.HWND_NOTOPMOST,
                0, 0, 0, 0,
                Win32Util.SWP_NOMOVE | Win32Util.SWP_NOSIZE);
        }
    }
}
