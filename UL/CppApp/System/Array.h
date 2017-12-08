#pragma once
#include "System\Object.h"
namespace System{
	struct Int32;
}
namespace System{
	class Array:public System::Object
	{
		public:
		System::Int32 get_Length();
	};
}
