#include "stdafx.h"
#include "String.h"
#include <windows.h>
using namespace System;
String::String(const wchar_t* wc)
{

	int len = WideCharToMultiByte(CP_ACP, 0, wc, wcslen(wc), NULL, 0, NULL, NULL);
	_v = new char[len + 1];
	WideCharToMultiByte(CP_ACP, 0, wc, wcslen(wc), _v, len, NULL, NULL);
	_v[len] = '\0';
}