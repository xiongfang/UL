#pragma once
#include "Object.h"

// TODO: 在此处引用程序需要的其他头文件
template<typename T>
class Ref
{
public:
	Handle v;
public:
	Ref(T* ptr)
	{
		if (ptr != nullptr)
		{
			ptr->AddRef();
			v = ptr->__GetID();
		}
		else
		{
			v = Handle();
		}
	}
	template<typename R>
	Ref(Ref<R>& c)
	{
		this->v = c.v;
		if (!IsNull())
			Get()->AddRef();
	}
	Ref()
	{
		v = Handle();
	}
	Ref(const Ref<T>& copy)
	{
		v = copy.v;
		if (!IsNull())
		{
			Get()->AddRef();
		}
	}
	~Ref()
	{
		if (!IsNull())
		{
			Get()->Release();
			v = Handle();
		}
	}
	T* operator->()
	{
		return Get();
	}

	T* Get() { return (T*)System::Object::Mgr.Get(v); }

	operator T*()
	{
		return Get();
	}

	Ref<T>& operator=(const Ref<T>& other)
	{
		v = other.v;
		if (!IsNull())
		{
			Get()->AddRef();
		}
		return *this;
	}

	bool IsNull()
	{
		return Get() == nullptr;
	}
};
