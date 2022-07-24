using Bubble.Model;
using Bubble.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bubble.View
{
    internal partial class PropertyWindow : Window
    {
        public PropertyWindow(WindowItem windowItem)
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            Title = $"属性 - {windowItem.Title}";
            this.windowItem = windowItem;
            handle.Text = HandleUtil.Format(windowItem.Handle);
            Loaded += PropertyWindow_Loaded;
        }

        private void PropertyWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= PropertyWindow_Loaded;
            title.Text = windowItem.Title;
            isTopmost.Text = windowItem.IsTopmost ? "是" : "否";
            _ = Win32Util.GetWindowThreadProcessId(windowItem.Handle, out uint pid);
            processId.Text = pid.ToString();
            processIcon.Handle = windowItem.Handle;
        }

        private void PropertyWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private readonly WindowItem windowItem;
    }
}
