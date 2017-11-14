#pragma once
namespace System
{
class String;
}
namespace System
{
class Object;
}
namespace System
{
class IFormatProvider;
}
namespace System{
	class ICustomFormater	{
		public:
		Ref<System::String> Format(Ref<System::String>  format,Ref<System::Object>  arg,Ref<System::IFormatProvider>  formatProvider);
	};
}
