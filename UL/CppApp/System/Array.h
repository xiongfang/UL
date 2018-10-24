#pragma once
#include "System\Object.h"
namespace System{
	struct Int32;
}
namespace System{
	class Array:public System::Object
	{
		public:
		virtual System::Int32 get_Length()=0;
	};
}
