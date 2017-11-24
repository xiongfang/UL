#pragma once
#include "Object.h"
namespace System{
	class Test:public System::Object
	{
		public:
		static void Run();
		private:
		static void TestInt();
		private:
		static void TestString();
	};
}
