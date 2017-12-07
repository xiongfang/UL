#pragma once
#include "System\Object.h"
namespace System{
	class TestDel;
}
namespace System{
	struct Boolean;
}
namespace System{
	class String;
}
namespace System{
	class Test:public System::Object
	{
		private:
		static 		Ref<System::TestDel> _notify;
		public:
		static void add_notify(Ref<System::TestDel>  value);
		public:
		static void remove_notify(Ref<System::TestDel>  value);
		public:
		static void Run();
		private:
		static void TestInt();
		private:
		static void TestString();
		private:
		static System::Boolean TestDel(Ref<System::String>  v);
		private:
		static void TestEvent();
		private:
		static System::Boolean Test_notify(Ref<System::String>  v);
	};
}
