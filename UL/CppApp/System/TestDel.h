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
		};
}
