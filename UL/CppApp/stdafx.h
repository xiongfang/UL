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
public:
	T* v;
public:
	Ref(T* ptr) 
	{ 
		this->v = ptr; 
		if (v != nullptr)
		{
			v->AddRef();
		}
	}
	template<typename R>
	Ref(Ref<R>& c) 
	{ 
		this->v = c.v; 
		if(v!=nullptr)
			v->AddRef();
	}
	Ref() 
	{ 
		v = nullptr;
	}
	Ref(const Ref<T>& copy)
	{ 
		v = copy.v;
		if (v != nullptr)
		{
			v->AddRef();
		}
	}
	~Ref()
	{
		if (v != nullptr)
		{
			v->Release();
			v = nullptr;
		}
	}
	T* operator->()
	{
		return v;
	}

	T* Get() { return v; }

	operator T*()
	{
		return Get();
	}

	Ref<T>& operator=(const Ref<T>& other)
	{
		v = other.v;
		if (v != nullptr)
		{
			v->AddRef();
		}
		return *this;
	}

	bool IsNull()
	{
		return v == nullptr;
	}
};

