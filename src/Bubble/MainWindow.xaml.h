#pragma once
#include "MainWindow.g.h"

namespace winrt::Bubble::implementation
{
    struct MainWindow : MainWindowT<MainWindow>
    {
        MainWindow();
        Bubble::VmMain Main() { return main; }

    private:
        Bubble::VmMain main;
        winrt::event_token activatedToken;
    };
}

namespace winrt::Bubble::factory_implementation
{
    struct MainWindow : MainWindowT<MainWindow, implementation::MainWindow>
    {
    };
}
