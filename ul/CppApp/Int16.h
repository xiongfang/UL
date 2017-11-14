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
	struct Int16:public System::Object
	{
		public:
		static 		System::Int16 MaxValue;
		public:
		static 		System::Int16 MinValue;
		public:
		static System::Int16 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Int16  v);
		public:
		virtual System::Boolean op_Equals(System::Int16  b);
		public:
		virtual System::Boolean op_Small(System::Int16  b);
		public:
		virtual System::Int16 op_Assign(System::Int16  b);
		public:
		virtual System::Int16 op_PlusPlus(System::Int16  b);
	};
}
