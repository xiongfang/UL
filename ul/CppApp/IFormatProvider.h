#pragma once
namespace System
{
class Object;
}
namespace System
{
class Type;
}
namespace System{
	class IFormatProvider	{
		public:
		Ref<System::Object> GetFormat(Ref<System::Type>  formatType);
	};
}
