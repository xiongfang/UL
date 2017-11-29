#pragma once
#include "Object.h"
#include "String.h"
namespace System{
		class Exception:public System::Object
		{
			private:
						Ref<System::String> _msg;
			public:
			Ref<System::String> get_Message();
			public:
			Exception(Ref<System::String>  msg);
			public:
			Exception();
		};
}
