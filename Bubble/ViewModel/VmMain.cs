using Bubble.Model;
using Bubble.Util;
using Bubble.View;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using Microsoft.Toolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bubble.ViewModel
{
    internal class VmMain : ObservableObject
    {
        public ObservableCollection<WindowItem> WindowItems => windowItems ??= new ObservableCollection<WindowItem>();

        public RelayCommand RefreshWindowListCommand => refreshWindowListCommand ??= new RelayCommand(RefreshWindowListAsync);

        public RelayCommand ClearSearchTextCommand => clearSearchTextCommand ??= new RelayCommand(ClearSearchText);

        public RelayCommand<IntPtr?> ResetPositionCommand => resetPositionCommand ??= new RelayCommand<IntPtr?>(ResetPosition);

        public RelayCommand<IntPtr?>? ResetSizeCommand => resetSizeCommand ??= new RelayCommand<IntPtr?>(ResetSize);

        public RelayCommand<WindowItem> PropertyCommand => propertyCommand ??= new RelayCommand<WindowItem>(ViewProperty);

        public RelayCommand AboutCommand => aboutCommand ??= new RelayCommand(About);

        public LoadingBox LoadingBox => loadingBox ??= new LoadingBox();

        public string SearchText
        {
            get => searchText;
            set
            {
                SetProperty(ref searchText, value);
                lock (lockObject)
                {
                    UpdateWindowItems(false);
                }
            }
        }

        private async void RefreshWindowListAsync()
        {
            LoadingBox.ShowLoading();
            await Application.Current.MainWindow.Dispatcher.BeginInvoke(() =>
            {
                lock (lockObject)
                {
                    UpdateWindowsList();
                    UpdateWindowItems(true);
                }
            });
            LoadingBox.HideLoading();
        }

        private void UpdateWindowItems(bool isUpdateProperties)
        {
            if (windowsList != null)
            {
                var tempSearchText = SearchText.ToLower();
                var list = windowsList.FindAll(t => t.Title.ToLower().Contains(tempSearchText));

                for (int i = WindowItems.Count - 1; i >= 0; --i)
                {
                    int j = list.Count - 1;
                    for (; j >= 0; --j)
                    {
                        if (WindowItems[i].Handle == list[j].Handle)
                        {
                            if (isUpdateProperties)
                            {
                                WindowItems[i].UpdateProperties(list[j].Title, list[j].IsTopmost);
                            }
                            list.RemoveAt(j);
                            break;
                        }
                    }
                    if (j == -1)
                    {
                        WindowItems.RemoveAt(i);
                    }
                }
                foreach (var item in list)
                {
                    WindowItems.Add(new WindowItem(item.Handle, item.Title, item.IsTopmost, SetTopmost));
                }
            }
        }

        private void UpdateWindowsList()
        {
            windowsList?.Clear();
            try
            {
                windowsList = WindowListUtil.GetWindowList();
            }
            catch (Exception ex)
            {
                var infoWindow = new InfoWindow("获取窗口列表失败", ex.Message);
                infoWindow.ShowDialog();
            }
        }

        private void ClearSearchText()
        {
            SearchText = string.Empty;
        }

        private bool SetTopmost(IntPtr hwnd, bool isTopmost)
        {
            if (TopmostUtil.SetIsTopmost(hwnd, isTopmost))
            {
                if (windowsList != null)
                {
                    var item = windowsList.FirstOrDefault(t => t.Handle == hwnd);
                    if (item != null)
                    {
                        item.IsTopmost = isTopmost;
                    }
                }
                return true;
            }
            var infoWindow = new InfoWindow($"{(isTopmost ? "" : "取消")}置顶操作失败", $"窗口句柄：{HandleUtil.Format(hwnd)}");
            infoWindow.ShowDialog();
            return false;
        }

        private void ViewProperty(WindowItem? windowItem)
        {
            if (windowItem != null)
            {
                var propertyWindow = new PropertyWindow(windowItem);
                propertyWindow.Show();
            }
        }

        private void About()
        {
            var aboutWindow = new AboutWindow();
            aboutWindow.ShowDialog();
        }

        private void ResetPosition(IntPtr? hwnd)
        {
            if (hwnd == null)
            {
                return;
            }
            try
            {
                if (!Win32Util.SetWindowPos((IntPtr)hwnd, IntPtr.Zero, 0, 0, 0, 0, Win32Util.SWP_NOSIZE))
                {
                    var infoWindow = new InfoWindow("重置位置失败", $"窗口句柄：{HandleUtil.Format((IntPtr)hwnd)}");
                    infoWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var infoWindow = new InfoWindow("重置位置失败", $"窗口句柄：{HandleUtil.Format((IntPtr)hwnd)}\r\n{ex.Message}");
                infoWindow.ShowDialog();
            }
        }

        private void ResetSize(IntPtr? hwnd)
        {
            if (hwnd == null)
            {
                return;
            }
            try
            {
                if (!Win32Util.SetWindowPos((IntPtr)hwnd, IntPtr.Zero, 0, 0, 500, 400, Win32Util.SWP_NOMOVE))
                {
                    var infoWindow = new InfoWindow("重置尺寸失败", $"窗口句柄：{HandleUtil.Format((IntPtr)hwnd)}");
                    infoWindow.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                var infoWindow = new InfoWindow("重置尺寸失败", $"窗口句柄：{HandleUtil.Format((IntPtr)hwnd)}\r\n{ex.Message}");
                infoWindow.ShowDialog();
            }
        }

        private RelayCommand? refreshWindowListCommand = null;
        private RelayCommand? clearSearchTextCommand = null;
        private RelayCommand<IntPtr?>? resetPositionCommand = null;
        private RelayCommand<IntPtr?>? resetSizeCommand = null;
        private RelayCommand<WindowItem>? propertyCommand = null;
        private RelayCommand? aboutCommand = null;
        private List<WindowListUtil.WindowInfo>? windowsList = null;
        private ObservableCollection<WindowItem>? windowItems = null;
        private LoadingBox? loadingBox = null;
        private string searchText = string.Empty;
        private readonly object lockObject = new();
    }
}
