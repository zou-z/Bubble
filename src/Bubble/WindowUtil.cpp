#include "pch.h"
#include <winrt/Microsoft.UI.Xaml.h>
#include "WindowUtil.h"
#include <microsoft.ui.xaml.window.h>
#include <ShObjIdl_core.h>
#include <winrt/Microsoft.UI.h>
#include <winrt/Microsoft.UI.Interop.h>
#include <winrt/Microsoft.UI.Windowing.h>
#include <winrt/Windows.Graphics.h>

using namespace winrt::Windows::Graphics;
using namespace winrt::Microsoft::UI;
using namespace winrt::Microsoft::UI::Xaml;
using namespace winrt::Microsoft::UI::Windowing;

void Util::SetWindowSize(Window const& window, int width, int height)
{
    if (width <= 0 || height <= 0) return;
    HWND hwnd{ 0 };
    window.try_as<IWindowNative>()->get_WindowHandle(&hwnd);
    auto windowId = GetWindowIdFromWindow(hwnd);
    auto appWindow = AppWindow::GetFromWindowId(windowId);
    appWindow.Resize(SizeInt32{ width,height });
}

void Util::SetWindowTitle(Window const& window, winrt::hstring const& title)
{
    HWND hwnd{ 0 };
    window.try_as<IWindowNative>()->get_WindowHandle(&hwnd);
    auto windowId = GetWindowIdFromWindow(hwnd);
    auto appWindow = AppWindow::GetFromWindowId(windowId);
    appWindow.Title(title);
}

void Util::SetWindowTitleAndSize(Window const& window, winrt::hstring const& title, int width, int height)
{
    HWND hwnd{ 0 };
    window.try_as<IWindowNative>()->get_WindowHandle(&hwnd);
    auto windowId = GetWindowIdFromWindow(hwnd);
    auto appWindow = AppWindow::GetFromWindowId(windowId);
    appWindow.Title(title);
    if (width > 0 && height > 0)
    {
        appWindow.Resize(SizeInt32{ width,height });
    }
}
