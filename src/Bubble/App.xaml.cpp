#include "pch.h"
#include "App.xaml.h"
#include "MainWindow.xaml.h"
#include "WindowUtil.h"

using namespace winrt;
using namespace Microsoft::UI::Xaml;
using namespace Microsoft::UI::Xaml::Controls;
using namespace Microsoft::UI::Xaml::Navigation;
using namespace Bubble;
using namespace Bubble::implementation;

App::App()
{
    InitializeComponent();

#if defined _DEBUG && !defined DISABLE_XAML_GENERATED_BREAK_ON_UNHANDLED_EXCEPTION
    UnhandledException([this](IInspectable const&, UnhandledExceptionEventArgs const& e)
    {
        if (IsDebuggerPresent())
        {
            auto errorMessage = e.Message();
            __debugbreak();
        }
    });
#endif
}

void App::OnLaunched(LaunchActivatedEventArgs const&)
{
    window = make<MainWindow>();
    Util::SetWindowTitleAndSize(window, L"Bubble", 350, 500);
    window.Activate();
}