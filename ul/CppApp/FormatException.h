#pragma once
#include "Exception.h"
namespace System
{
class String;
}
namespace System{
	class FormatException:public System::Exception
	{
		public:
		FormatException(Ref<System::String>  msg);
		public:
		FormatException();
	};
}
