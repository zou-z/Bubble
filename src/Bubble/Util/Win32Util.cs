using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bubble.Util
{
    internal class Win32Util
    {
        public delegate bool EnumWindowsProc(IntPtr hwnd, IntPtr lParam);
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool EnumWindows(EnumWindowsProc lpEnumFunc, IntPtr lParam);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowTextLength(IntPtr hwnd);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hwnd, StringBuilder lpString, int cch);


        [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern int GetClassName(IntPtr hwnd, StringBuilder lpClassName, int nMaxCount);


        [DllImport("user32.dll")]
        public static extern IntPtr GetShellWindow();


        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hwnd);


        public const uint GA_ROOT = 2; // Retrieves the root window by walking the chain of parent windows.
        [DllImport("user32.dll")]
        public static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);


        public const int GWL_STYLE = -16; // Retrieves the window styles.
        public const int GWL_EXSTYLE = -20; // Retrieves the extended window styles.
        public const long WS_DISABLED = 0x08000000L; // The window is initially disabled. A disabled window cannot receive input from the user.
        public const long WS_EX_TOOLWINDOW = 0x00000080L; // The window is intended to be used as a floating toolbar.
        [DllImport("user32.dll")]
        public static extern long GetWindowLong(IntPtr hwnd, int nIndex);


        public const int DWM_CLOAKED_SHELL = 0x0000002; // The window was cloaked by the Shell.
        public enum DWMWINDOWATTRIBUTE
        {
            DWMWA_NCRENDERING_ENABLED = 1,
            DWMWA_NCRENDERING_POLICY,
            DWMWA_TRANSITIONS_FORCEDISABLED,
            DWMWA_ALLOW_NCPAINT,
            DWMWA_CAPTION_BUTTON_BOUNDS,
            DWMWA_NONCLIENT_RTL_LAYOUT,
            DWMWA_FORCE_ICONIC_REPRESENTATION,
            DWMWA_FLIP3D_POLICY,
            DWMWA_EXTENDED_FRAME_BOUNDS,
            DWMWA_HAS_ICONIC_BITMAP,
            DWMWA_DISALLOW_PEEK,
            DWMWA_EXCLUDED_FROM_PEEK,
            DWMWA_CLOAK,
            DWMWA_CLOAKED,
            DWMWA_FREEZE_REPRESENTATION,
            DWMWA_LAST
        };
        [DllImport("dwmapi.dll")]
        public static extern uint DwmGetWindowAttribute(IntPtr hwnd, DWMWINDOWATTRIBUTE dwAttribute, out int pvAttribute, int cbAttribute);


        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hwnd, uint msg, IntPtr wParam, IntPtr lParam);


        [DllImport("user32.dll")]
        public static extern IntPtr LoadIcon(IntPtr hInstance, IntPtr lpIconName);


        [DllImport("user32.dll", EntryPoint = "GetClassLong")]
        public static extern uint GetClassLong32(IntPtr hwnd, int nIndex);


        [DllImport("user32.dll", EntryPoint = "GetClassLongPtr")]
        public static extern IntPtr GetClassLong64(IntPtr hwnd, int nIndex);

        public const uint WM_GETICON = 0x007f;
        public static readonly IntPtr ICON_SMALL2 = new(2);
        public static readonly IntPtr IDI_APPLICATION = new(0x7F00);
        public const int GCL_HICON = -14;

        public const long WS_EX_TOPMOST = 0x00000008L;

        public static readonly IntPtr HWND_TOPMOST = new(-1);
        public static readonly IntPtr HWND_NOTOPMOST = new(-2);
        public const uint SWP_NOMOVE = 0x0002;
        public const uint SWP_NOSIZE = 0x0001;

        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hwnd, IntPtr hwndInsertAfter, int x, int y, int cx, int cy, uint uFlags);


        [DllImport("user32.dll")]
        public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(uint dwDesiredAccess, int bInheritHandle, uint dwProcessId);

        [DllImport("psapi.dll", CharSet = CharSet.Unicode)]
        public static extern uint GetModuleFileNameEx(IntPtr hProcess, IntPtr hModule, [Out] StringBuilder lpBaseName, [In][MarshalAs(UnmanagedType.U4)] int nSize);

        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool CloseHandle(IntPtr hObject);

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hwnd);
    }
}
