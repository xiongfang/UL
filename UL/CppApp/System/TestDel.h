#pragma once
#include "System\Object.h"
namespace System{
	struct Boolean;
}
namespace System{
	class String;
}
namespace System{
	
		class TestDel:public System::Object
		{
			public:
			System::Boolean Invoke(Ref<System::String>  v);

			Ref<System::Object> object;
			typedef System::Boolean(Type)(Ref<System::String>  v);
			Type* p;

			TestDel(Ref<System::Object> object, Type* t)
			{
				p = t;
				this->object = object;
			}
		};
}
