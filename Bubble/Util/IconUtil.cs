using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml.Linq;

namespace Bubble.Util
{
    internal class IconUtil
    {
        public static BitmapSource? GetIcon(IntPtr hwnd)
        {
            IntPtr icon = Win32Util.SendMessage(hwnd, Win32Util.WM_GETICON, Win32Util.ICON_SMALL2, IntPtr.Zero);
            if (icon == IntPtr.Zero)
            {
                if (IntPtr.Size == 4)
                {
                    icon = new IntPtr(Win32Util.GetClassLong32(hwnd, Win32Util.GCL_HICON));
                }
                else
                {
                    icon = Win32Util.GetClassLong64(hwnd, Win32Util.GCL_HICON);
                }
            }
            if (icon == IntPtr.Zero)
            {
                var bitmap = GetModernAppLogo(hwnd);
                if (bitmap != null)
                {
                    return bitmap;
                }
            }
            if (icon == IntPtr.Zero)
            {
                icon = Win32Util.LoadIcon(IntPtr.Zero, Win32Util.IDI_APPLICATION);
            }
            if (icon != IntPtr.Zero)
            {
                var bitmap = new System.Drawing.Bitmap(System.Drawing.Icon.FromHandle(icon).ToBitmap(), 32, 32);
                var writeableBitmap = BitmapToWriteableBitmap(bitmap);
                bitmap.Dispose();
                return writeableBitmap;
            }
            else
            {
                return null;
            }
        }

        private static BitmapImage? GetModernAppLogo(IntPtr hwnd)
        {
            // 获取进程exe文件位置
            var builder = new StringBuilder(1024);
            _ = Win32Util.GetWindowThreadProcessId(hwnd, out uint processId);
            IntPtr hProcess = Win32Util.OpenProcess(1040, 0, processId);
            _ = Win32Util.GetModuleFileNameEx(hProcess, IntPtr.Zero, builder, builder.Capacity);
            Win32Util.CloseHandle(hProcess);
            var exePath = builder.ToString();

            // 获取exe所在的上一层文件夹
            var dir = exePath[..exePath.LastIndexOf('\\')];
            dir = exePath[..dir.LastIndexOf('\\')];

            var manifestPath = Path.Combine(dir, "AppxManifest.xml");
            if (File.Exists(manifestPath))
            {
                string? pathToLogo = null;
                using (var fs = File.OpenRead(manifestPath))
                {
                    XDocument? manifest = XDocument.Load(fs);
                    const string ns = "http://schemas.microsoft.com/appx/manifest/foundation/windows10";
                    XElement? element = manifest?.Root?.Element(XName.Get("Properties", ns));
                    if (element != null)
                    {
                        pathToLogo = element.Element(XName.Get("Logo", ns))?.Value;
                    }
                }

                string? finalLogo = null;
                string? dirName = Path.GetDirectoryName(pathToLogo);
                if (dirName != null)
                {
                    foreach (var logoFile in Directory.GetFiles(Path.Combine(dir, dirName), Path.GetFileNameWithoutExtension(pathToLogo) + "*" + Path.GetExtension(pathToLogo)))
                    {
                        finalLogo = logoFile;
                        break;
                    }
                }

                if (File.Exists(finalLogo))
                {
                    using var fs = File.OpenRead(finalLogo);
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.StreamSource = fs;
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.EndInit();
                    return img;
                }
            }
            return null;
        }

        private static WriteableBitmap BitmapToWriteableBitmap(System.Drawing.Bitmap bitmap)
        {
            int stride = bitmap.Width * 4;
            byte[] pixels = new byte[bitmap.Height * stride];
            for (int y = 0, index = 0; y < bitmap.Height; ++y)
            {
                for (int x = 0; x < bitmap.Width; ++x)
                {
                    var color = bitmap.GetPixel(x, y);
                    pixels[index++] = color.B;
                    pixels[index++] = color.G;
                    pixels[index++] = color.R;
                    pixels[index++] = color.A;
                }
            }

            var writeableBitmap = new WriteableBitmap(bitmap.Width, bitmap.Height, 72, 72, PixelFormats.Bgra32, null);
            writeableBitmap.Lock();
            writeableBitmap.WritePixels(new Int32Rect(0, 0, bitmap.Width, bitmap.Height), pixels, stride, 0);
            writeableBitmap.Unlock();
            return writeableBitmap;
        }
    }
}
