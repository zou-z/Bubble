#include "pch.h"
#include <string>
#include "StringUtil.h"
#include <cwctype>

//void Util::ToLower(std::wstring& str)
//{
//	if (str.size() == 0)return;
//	for (int i = 0; i < str.size(); ++i)
//	{
//		str[i] = std::towlower(str[i]);
//	}
//}

std::wstring Util::ToLower(std::wstring const& str)
{
	std::wstring result = str;
	if (str.size() > 0)
	{
		for (uint64_t i = 0; i < str.size(); ++i)
		{
			result[i] = std::towlower(str[i]);
		}
	}
	return result;
}
