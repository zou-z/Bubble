#pragma once
#include <functional>
#include <winrt/Windows.Foundation.h>
#include <winrt/Microsoft.UI.Xaml.Data.h>
#include <winrt/Microsoft.UI.Xaml.Input.h>

namespace Util
{
    struct NotifyBase
    {
        template<typename T> void Set(T& field, T newValue, winrt::Windows::Foundation::IInspectable const& sender, winrt::param::hstring const& name)
        {
            field = newValue;
            propertyChanged(sender, winrt::Microsoft::UI::Xaml::Data::PropertyChangedEventArgs{ name });
        }

        winrt::event_token PropertyChanged(winrt::Microsoft::UI::Xaml::Data::PropertyChangedEventHandler const& handler)
        {
            return propertyChanged.add(handler);
        }

        void PropertyChanged(winrt::event_token const& token)
        {
            propertyChanged.remove(token);
        }

        winrt::event<winrt::Microsoft::UI::Xaml::Data::PropertyChangedEventHandler> propertyChanged;
    };

    struct RelayCommand : winrt::implements<RelayCommand, winrt::Microsoft::UI::Xaml::Input::ICommand>
    {
        winrt::event<winrt::Windows::Foundation::EventHandler<winrt::Windows::Foundation::IInspectable>> canExecuteChanged;
        using ExecuteCallback = std::function<void(winrt::Windows::Foundation::IInspectable)>;
        using CanExecuteCallback = std::function<bool(winrt::Windows::Foundation::IInspectable)>;

        RelayCommand(ExecuteCallback execute) : RelayCommand(execute, nullptr) {}

        RelayCommand(ExecuteCallback execute, CanExecuteCallback canExecute)
        {
            this->execute = execute;
            this->canExecute = canExecute;
        }

        void Execute(winrt::Windows::Foundation::IInspectable const& parameter)
        {
            if (execute != nullptr)
            {
                execute(parameter);
            }
        }

        bool CanExecute(winrt::Windows::Foundation::IInspectable const& parameter)
        {
            return canExecute == nullptr || canExecute(parameter);
        }

        winrt::event_token CanExecuteChanged(winrt::Windows::Foundation::EventHandler<winrt::Windows::Foundation::IInspectable> const& handler)
        {
            return canExecuteChanged.add(handler);
        }

        void CanExecuteChanged(winrt::event_token const& token)
        {
            canExecuteChanged.remove(token);
        }
    private:
        ExecuteCallback execute;
        CanExecuteCallback canExecute;
    };
}
