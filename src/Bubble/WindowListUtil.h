#pragma once

namespace Util
{
	struct WindowInfo
	{
		HWND WindowHandle;
		std::wstring Title;
		std::wstring ClassName;
		bool IsTopmost = false;

		bool operator==(const WindowInfo& info) { return WindowHandle == info.WindowHandle; }
		//bool operator!=(const WindowInfo& info) { return !(*this == info); }
	};

	std::vector<WindowInfo>* GetWindowList();
}
