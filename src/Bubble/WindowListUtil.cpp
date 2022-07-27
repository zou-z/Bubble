#include "pch.h"
#include "WindowListUtil.h"
#include <dwmapi.h>

bool inline MatchTitleAndClassName(std::wstring const& title, std::wstring const& className, std::wstring const& targetTitle, std::wstring const& targetClassName)
{
	return wcscmp(title.c_str(), targetTitle.c_str()) == 0 && wcscmp(className.c_str(), targetClassName.c_str()) == 0;
}

bool IsKnownBlockedWindow(std::wstring const& title, std::wstring const& className)
{
	return MatchTitleAndClassName(title, className, L"Task View", L"Windows.UI.Core.CoreWindow") || // Task View 
		MatchTitleAndClassName(title, className, L"DesktopWindowXamlSource", L"Windows.UI.Core.CoreWindow") || // XAML Islands
		MatchTitleAndClassName(title, className, L"PopupHost", L"Xaml_WindowedPopupClass"); // XAML Popups
}

bool IsCaptureableWindow(HWND const& hwnd, std::wstring const& title, std::wstring const& className, bool& isTopmost)
{
    if (title.empty() || hwnd == GetShellWindow() || !IsWindowVisible(hwnd) || GetAncestor(hwnd, GA_ROOT) != hwnd)
    {
        return false;
    }

    if (GetWindowLongW(hwnd, GWL_STYLE) & WS_DISABLED)
    {
        return false;
    }

    auto exStyle = GetWindowLongW(hwnd, GWL_EXSTYLE);
    if (exStyle & WS_EX_TOOLWINDOW)
    {
        return false;
    }

    if (wcscmp(className.c_str(), L"Windows.UI.Core.CoreWindow") == 0 || wcscmp(className.c_str(), L"ApplicationFrameWindow") == 0)
    {
        DWORD cloaked = FALSE;
        if (SUCCEEDED(DwmGetWindowAttribute(hwnd, DWMWA_CLOAKED, &cloaked, sizeof(cloaked))) && (cloaked == DWM_CLOAKED_SHELL))
        {
            return false;
        }
    }

    if (IsKnownBlockedWindow(title, className))
    {
        return false;
    }

    isTopmost = exStyle & WS_EX_TOPMOST;
    return true;
}

std::vector<Util::WindowInfo>* Util::GetWindowList()
{
	std::vector<Util::WindowInfo>* list = new std::vector<Util::WindowInfo>();
	EnumWindows([](HWND hwnd, LPARAM lParam) 
	{
		int titleLength = GetWindowTextLengthW(hwnd);
		if (titleLength > 0)
		{
			std::wstring title(++titleLength, 0);
			if (GetWindowTextW(hwnd, LPWSTR(title.data()), titleLength) == 0) return TRUE;
			int classNameLength = 256;
			std::wstring className(classNameLength, 0);
			if (GetClassNameW(hwnd, LPWSTR(className.data()), classNameLength) == 0)return TRUE;
            bool isTopmost = false;
            if (IsCaptureableWindow(hwnd, title, className, isTopmost))
            {
                std::vector<Util::WindowInfo>* list = reinterpret_cast<std::vector<Util::WindowInfo>*>(lParam);
                list->push_back(Util::WindowInfo{ hwnd,title,className,isTopmost });
            }
		}
		return TRUE;
	}, reinterpret_cast<LPARAM>(list));
	return list;
}

void Util::GetWindowList(std::vector<Util::WindowInfo> const& list)
{
    EnumWindows([](HWND hwnd, LPARAM lParam)
    {
        int titleLength = GetWindowTextLengthW(hwnd);
        if (titleLength > 0)
        {
            std::wstring title(++titleLength, 0);
            if (GetWindowTextW(hwnd, LPWSTR(title.data()), titleLength) == 0) return TRUE;
            int classNameLength = 256;
            std::wstring className(classNameLength, 0);
            if (GetClassNameW(hwnd, LPWSTR(className.data()), classNameLength) == 0)return TRUE;
            bool isTopmost = false;
            if (IsCaptureableWindow(hwnd, title, className, isTopmost))
            {
                std::vector<Util::WindowInfo>* list = reinterpret_cast<std::vector<Util::WindowInfo>*>(lParam);
                list->push_back(Util::WindowInfo{ hwnd,title,className,isTopmost });
            }
        }
        return TRUE;
    }, reinterpret_cast<LPARAM>(&list));
}
