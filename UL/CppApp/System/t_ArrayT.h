#pragma once
#include "System\Array.h"
namespace System{
	struct Int32;
}
namespace System{
	template<class T>
	class ArrayT:public System::Array
	{
		public:
		Ref<T> get_Index(System::Int32  i)		public:
		void set_Index(System::Int32  i)		public:
		ArrayT(System::Int32  len)		{
		}
	};
}
