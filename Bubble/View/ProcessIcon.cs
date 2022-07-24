using Bubble.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Bubble.View
{
    public class ProcessIcon : Image
    {
        public IntPtr Handle
        {
            get => (IntPtr)GetValue(HandleProperty);
            set => SetValue(HandleProperty, value);
        }

        private static void HandleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProcessIcon self && self != null)
            {
                IntPtr intPtr = (IntPtr)e.NewValue;
                if (intPtr != IntPtr.Zero)
                {
                    try
                    {
                        self.Source = IconUtil.GetIcon(intPtr);
                    }
                    catch (Exception ex)
                    {
                        var infoWindow = new InfoWindow("获取窗口图标失败", $"窗口句柄：{HandleUtil.Format(intPtr)}\r\n{ex.Message}");
                        infoWindow.ShowDialog();
                    }
                }
            }
        }

        public static readonly DependencyProperty HandleProperty = DependencyProperty.Register("Handle", typeof(IntPtr), typeof(ProcessIcon), new PropertyMetadata(IntPtr.Zero, new PropertyChangedCallback(HandleChanged)));
    }
}
