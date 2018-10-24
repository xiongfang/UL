#pragma once
#include "System\Array.h"
#include "System\Int32.h"
namespace System{
	template<class T>
	class ArrayT:public System::Array
	{
	#include "Array_Ext.h"
	};
}
