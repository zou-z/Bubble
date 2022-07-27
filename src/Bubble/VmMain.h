#pragma once
#include "VmMain.g.h"
#include "MvvmUtil.h"
#include "WindowItem.h"
#include "WindowListUtil.h"

namespace winrt::Bubble::implementation
{
    struct VmMain : VmMainT<VmMain>, Util::NotifyBase
    {
        VmMain();
        winrt::hstring SearchText() { return searchText; }
        void SearchText(winrt::hstring const& value);
        winrt::Windows::Foundation::Collections::IObservableVector<Bubble::WindowItem> WindowList() { return windowList; }
        winrt::Microsoft::UI::Xaml::Input::ICommand RefreshWindowListCommand() { return refreshWindowListCommand; }

    private:
        winrt::hstring searchText;
        winrt::Windows::Foundation::Collections::IObservableVector<Bubble::WindowItem> windowList;
        winrt::Microsoft::UI::Xaml::Input::ICommand refreshWindowListCommand;
        std::vector<Util::WindowInfo> windowListSource;

    private:
        winrt::Windows::Foundation::IAsyncAction RefreshWindowList(winrt::Windows::Foundation::IInspectable const&);
        bool UpdateWindowListSource();
        void UpdateWindowList(bool isUpdateProperties);
    };
}

namespace winrt::Bubble::factory_implementation
{
    struct VmMain : VmMainT<VmMain, implementation::VmMain>
    {
    };
}
