#pragma once
#include "Object.h"
namespace System
{
class String;
}
namespace System
{
struct Boolean;
}
namespace System{
	struct UInt32:public System::Object
	{
		public:
		static 		System::UInt32 MaxValue;
		public:
		static 		System::UInt32 MinValue;
		public:
		static System::UInt32 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::UInt32 & v);
	#include "UInt32_ExtHeader.h"
	};
}
