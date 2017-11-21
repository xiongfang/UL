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
	struct UInt16:public System::Object
	{
		public:
		static 		System::UInt16 MaxValue;
		public:
		static 		System::UInt16 MinValue;
		public:
		static System::UInt16 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::UInt16  v);
		public:
		virtual System::Boolean op_Small(System::UInt16  b);
		public:
		virtual System::UInt16 op_PlusPlus(System::UInt16  b);
	};
}
