#include "stdafx.h"
#include "String.h"
#include <windows.h>
using namespace System;
String::String(const wchar_t* c)
{
	int size = wcslen(c) + 1;
	_v = new wchar_t[size];
	wmemset(_v, 0, size);
	wsprintf(_v, _T("%s"), c);
	_length = size - 1;
}

String::String(const char* wc)
{
	//获取输入缓存大小
	int sBufSize = strlen(wc);
	//获取输出缓存大小
	//VC++ 默认使用ANSI，故取第一个参数为CP_ACP
	DWORD dBufSize = MultiByteToWideChar(CP_ACP, 0, wc, sBufSize, NULL, 0)+1;
	_v = new wchar_t[dBufSize];
	wmemset(_v, 0, dBufSize);

	//进行转换
	int nRet = MultiByteToWideChar(CP_ACP, 0, wc, sBufSize, _v, dBufSize);

	_length = dBufSize-1;
}