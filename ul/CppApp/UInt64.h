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
	struct UInt64:public System::Object
	{
		public:
		static 		System::UInt64 MaxValue;
		public:
		static 		System::UInt64 MinValue;
		public:
		static System::UInt64 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::UInt64 & v);
	#include "UInt64_ExtHeader.h"
	};
}
