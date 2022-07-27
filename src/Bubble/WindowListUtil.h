#pragma once

namespace Util
{
	struct WindowInfo
	{
		HWND WindowHandle{ 0 };
		std::wstring Title = L"";
		std::wstring ClassName = L"";
		bool IsTopmost = false;

		bool operator==(const WindowInfo& info) { return WindowHandle == info.WindowHandle; }
		//bool operator!=(const WindowInfo& info) { return !(*this == info); }
	};

	std::vector<WindowInfo>* GetWindowList();
	void GetWindowList(std::vector<WindowInfo> const& list);
}
