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
	struct Byte:public System::Object
	{
		public:
		static 		System::Byte MaxValue;
		public:
		static 		System::Byte MinValue;
		public:
		static System::Byte Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Byte  v);
		public:
		virtual System::Boolean op_Equals(System::Byte  b);
		public:
		virtual System::Boolean op_Small(System::Byte  b);
		public:
		virtual System::Byte op_Assign(System::Byte  b);
		public:
		virtual System::Byte op_PlusPlus(System::Byte  b);
	};
}
