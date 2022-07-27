#pragma once
#include "WindowItem.g.h"
#include "MvvmUtil.h"

namespace winrt::Bubble::implementation
{
    struct WindowItem : WindowItemT<WindowItem>, Util::NotifyBase
    {
        WindowItem(int64_t windowHandle, winrt::hstring title, bool isTopmost);
        int64_t Handle() { return handle; }
        winrt::hstring Title() { return title; }
        void Title(winrt::hstring value) { Set(title, value, *this, L"Title"); }
        bool IsTopmost() { return isTopmost; }
        void IsTopmost(bool value);
        bool SetTopmostEnable() { return setTopmostEnable; }
        void UpdateProperties(winrt::hstring title, bool isTopmost);

    private:
        int64_t handle;
        winrt::hstring title;
        bool isTopmost;
        bool setTopmostEnable;
    };
}

namespace winrt::Bubble::factory_implementation
{
    struct WindowItem : WindowItemT<WindowItem, implementation::WindowItem>
    {
    };
}
