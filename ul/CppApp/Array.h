#pragma once
#include "Object.h"
namespace System
{
struct Int32;
}
namespace System{
	template<class T>
	class Array:public System::Object
	{
		public:
		Array(System::Int32  len)
		{
		}
	};
}
