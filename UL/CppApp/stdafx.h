// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件
//

#pragma once

#include "targetver.h"

#include <stdio.h>
#include <tchar.h>

#include <string.h>

#include "HandleManager.h"

template<typename T>
class Ref;

#include "Ref.h"

class PostfixUnaryHelper
{
public:

	template<typename T>
	static T op_Increment(T& a)
	{
		T temp = a;
		a = T::op_Increment(a);
		return temp;
	}

	template<typename T>
	static T op_Decrement(T& a)
	{
		T temp = a;
		a = T::op_Decrement(a);
		return temp;
	}
};

class PrefixUnaryHelper
{
public:

	template<typename T>
	static T op_Increment(T& a)
	{
		a = T::op_Increment(a);
		return a;
	}

	template<typename T>
	static T op_Decrement(T& a)
	{
		a = T::op_Decrement(a);
		return a;
	}
};