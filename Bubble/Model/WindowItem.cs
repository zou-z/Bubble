using Bubble.Util;
using Bubble.View;
using Microsoft.Toolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bubble.Model
{
    internal class WindowItem : ObservableObject
    {
        public IntPtr Handle { get; set; }

        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        public bool IsTopmost
        {
            get => isTopmost;
            set
            {
                IsTopmostEnable = false;
                if (setTopmostDelegate(Handle, value))
                {
                    SetProperty(ref isTopmost, value);
                }
                IsTopmostEnable = true;
            }
        }
        
        public bool IsTopmostEnable
        {
            get => isTopmostEnable;
            set => SetProperty(ref isTopmostEnable, value);
        }

        public WindowItem(IntPtr handle, string title, bool isTopmost, Func<IntPtr, bool, bool> setTopmostDelegate)
        {
            Handle = handle;
            this.title = title;
            this.isTopmost = isTopmost;
            this.setTopmostDelegate = setTopmostDelegate;
            isTopmostEnable = true;
        }

        public void UpdateProperties(string title, bool isTopmost)
        {
            SetProperty(ref this.title, title, nameof(Title));
            SetProperty(ref this.isTopmost, isTopmost, nameof(IsTopmost));
        }

        private string title;
        private bool isTopmost;
        private bool isTopmostEnable;
        private Func<IntPtr, bool, bool> setTopmostDelegate;
    }
}
