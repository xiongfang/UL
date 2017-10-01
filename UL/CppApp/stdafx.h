// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>

#include <string.h>

// TODO: 在此处引用程序需要的其他头文件
template<typename T>
class Ref
{
	T* v;
public:
	Ref(T* ptr) { this->v = ptr; }
	Ref() { v = nullptr; }
	T* operator->()
	{
		return v;
	}

	T* Get() { return v; }
};