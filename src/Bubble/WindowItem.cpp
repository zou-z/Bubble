#include "pch.h"
#include "WindowItem.h"
#if __has_include("WindowItem.g.cpp")
#include "WindowItem.g.cpp"
#endif

namespace winrt::Bubble::implementation
{
	WindowItem::WindowItem(int64_t windowHandle, winrt::hstring title, bool isTopmost)
	{
		handle = windowHandle;
		this->title = title;
		this->isTopmost = isTopmost;
		setTopmostEnable = true;
	}

	void WindowItem::IsTopmost(bool value)
	{
		Set(setTopmostEnable, false, *this, L"SetTopmostEnable");
		Set(isTopmost, value, *this, L"IsTopmost");
		// call win32

		Set(setTopmostEnable, true, *this, L"SetTopmostEnable");
	}

	void WindowItem::UpdateProperties(winrt::hstring _title, bool _isTopmost)
	{
		Set(title, _title, *this, L"Title");
		Set(isTopmost, _isTopmost, *this, L"IsTopmost");
	}
}
