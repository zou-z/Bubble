using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Bubble.Model
{
    internal class LoadingBox : ObservableObject
    {
        public Visibility LoadingVisibility
        {
            get => loadingVisibility;
            set => SetProperty(ref loadingVisibility, value);
        }

        public void ShowLoading()
        {
            LoadingVisibility = Visibility.Visible;
        }

        public void HideLoading()
        {
            LoadingVisibility = Visibility.Collapsed;
        }

        private Visibility loadingVisibility = Visibility.Collapsed;
    }
}
