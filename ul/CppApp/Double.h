#pragma once
#include "Object.h"
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
		virtual System::Boolean op_Small(System::Double  b);
		public:
		virtual System::Double op_PlusPlus(System::Double  b);
	};
}
