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
	struct Single:public System::Object
	{
		public:
		static 		System::Single Epsilon;
		public:
		static 		System::Single MaxValue;
		public:
		static 		System::Single MinValue;
		public:
		static System::Single Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Single & v);
	#include "Single_ExtHeader.h"
	};
}
