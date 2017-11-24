#pragma once
#include "Object.h"
#include "Array.h"
namespace System{
	class String:public System::Object
	{
		public:
		static Ref<System::String> Format(Ref<System::String>  format,Ref<System::Array<System::Object>>  args);
	#include "String_ExtHeader.h"
	};
}
