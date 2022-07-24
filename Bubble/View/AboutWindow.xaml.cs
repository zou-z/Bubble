using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
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
    internal partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            Owner = Application.Current.MainWindow;
            appName.Text = Assembly.GetExecutingAssembly().GetName().Name;
            appVersion.Text = $"v{Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void AboutWindow_MouseMove(object sender, MouseEventArgs e)
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

        private void Github_Click(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (sender is TextBlock text && text != null && text.ToolTip is string url)
                {
                    Process.Start("explorer.exe", url);
                }
            }
            catch (Exception ex)
            {
                var infoWindow = new InfoWindow("操作失败", ex.Message);
                infoWindow.ShowDialog();
            }
        }
    }
}
