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
	struct Double:public System::Object
	{
		public:
		static 		System::Double Epsilon;
		public:
		static 		System::Double MaxValue;
		public:
		static 		System::Double MinValue;
		public:
		static System::Double Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Double  v);
	#include "Double_ExtHeader.h"
	};
}
