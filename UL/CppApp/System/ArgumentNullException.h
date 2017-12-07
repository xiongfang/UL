#pragma once
#include "System\Exception.h"
namespace System{
	class String;
}
namespace System{
	class ArgumentNullException:public System::Exception
	{
		public:
		ArgumentNullException(Ref<System::String>  msg);
		public:
		ArgumentNullException();
	};
}
