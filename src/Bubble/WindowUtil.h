#pragma once

namespace Util
{
	void SetWindowSize(winrt::Microsoft::UI::Xaml::Window const& window, int width, int height);

	void SetWindowTitle(winrt::Microsoft::UI::Xaml::Window const& window, winrt::hstring const& title);

	void SetWindowTitleAndSize(winrt::Microsoft::UI::Xaml::Window const& window, winrt::hstring const& title, int width, int height);
}
