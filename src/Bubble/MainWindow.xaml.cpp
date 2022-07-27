#include "pch.h"
#include "VmMain.h"
#include "MainWindow.xaml.h"
#if __has_include("MainWindow.g.cpp")
#include "MainWindow.g.cpp"
#endif

#include <winrt/Microsoft.UI.Xaml.Input.h>

using namespace winrt::Windows::Foundation;
using namespace winrt::Microsoft::UI::Xaml;

namespace winrt::Bubble::implementation
{
    MainWindow::MainWindow()
    {
        InitializeComponent();
        main = winrt::make<Bubble::implementation::VmMain>();
        activatedToken = Activated([this](IInspectable, WindowActivatedEventArgs)
        {
            Activated(activatedToken);
            main.RefreshWindowListCommand().Execute(IInspectable{});
        });
    }
}
