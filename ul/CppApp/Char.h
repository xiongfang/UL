#pragma once
#include "Object.h"
namespace System{
	class String;
}
namespace System{
	struct Boolean;
}
namespace System{
		struct Char:public System::Object
		{
			public:
			static 			System::Char MaxValue;
			public:
			static 			System::Char MinValue;
			public:
			static System::Char Parse(Ref<System::String>  value);
			public:
			Ref<System::String> ToString();
			public:
			static System::Boolean TryParse(Ref<System::String>  value,System::Char & v);
		#include "Char_ExtHeader.h"
		};
}
