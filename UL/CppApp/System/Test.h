#pragma once
#include "System\Object.h"
namespace System{
	struct Boolean;
}
namespace System{
	class String;
}
namespace System{
		class Test:public System::Object
		{
			public:
			static void Run();
			private:
			static void TestInt();
			private:
			static void TestString();
			private:
			static void TestDel();
			private:
			static System::Boolean TestDel(Ref<System::String>  v);
		};
}
