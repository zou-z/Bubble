#include "pch.h"
#include <winrt/Microsoft.UI.Xaml.Input.h>
#include "VmMain.h"
#if __has_include("VmMain.g.cpp")
#include "VmMain.g.cpp"
#endif

#include "StringUtil.h"
#include "WindowItem.h"

using namespace winrt::Windows::Foundation;

namespace winrt::Bubble::implementation
{
	VmMain::VmMain()
	{
		searchText = L"";
		windowList = winrt::single_threaded_observable_vector<Bubble::WindowItem>();
		refreshWindowListCommand = winrt::make<Util::RelayCommand>(std::bind(&VmMain::RefreshWindowList, this, std::placeholders::_1));
	}

	void VmMain::SearchText(winrt::hstring const& value)
	{
		Set(searchText, value, *this, L"SearchText");

	}

	IAsyncAction VmMain::RefreshWindowList(IInspectable const&)
	{
		// show loading
		UpdateWindowListSource();
		UpdateWindowList(true);
		// hide loading
		co_return;
	}

	bool VmMain::UpdateWindowListSource()
	{
		try
		{
			windowListSource.clear();
			Util::GetWindowList(windowListSource);
			return true;
		}
		catch (...)
		{
			return false;
		}
	}

	void VmMain::UpdateWindowList(bool isUpdateProperties)
	{
		if (windowListSource.size() > 0)
		{
			std::wstring lowerSearchText = Util::ToLower(std::wstring(SearchText()));
			std::vector<Util::WindowInfo> list;
			for (uint64_t i = 0; i < windowListSource.size(); ++i)
			{
				if (Util::ToLower(windowListSource[i].Title).find(lowerSearchText) != std::wstring::npos)
				{
					list.push_back(windowListSource[i]);
				}
			}

			for (int32_t i = WindowList().Size() - 1; i >= 0; --i)
			{
				int64_t j = list.size() - 1;
				for (; j >= 0; --j)
				{
					if (WindowList().GetAt(i).Handle() == (int64_t)list[j].WindowHandle)
					{
						if (isUpdateProperties)
						{


						}
						list.erase(list.begin() + j);
						break;
					}
				}
				if (j == -1)
				{
					WindowList().RemoveAt(i);
				}
			}

			for (int i = 0; i < list.size(); ++i)
			{
				Bubble::WindowItem item = winrt::make<Bubble::implementation::WindowItem>(
					(int64_t)list[i].WindowHandle,
					winrt::hstring(list[i].Title),
					list[i].IsTopmost);
				// item handler
				WindowList().Append(item);
			}
			std::vector<Util::WindowInfo>(0).swap(list);
		}
	}
}
