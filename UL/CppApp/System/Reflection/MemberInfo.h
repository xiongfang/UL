#pragma once
#include "System\Object.h"
namespace System{
	namespace Reflection{
		struct MemberTypes;
	}
}
namespace System{
	class String;
}
namespace System{
	class Type;
}
namespace System{
	namespace Reflection{
			class MemberInfo:public System::Object
			{
				public:
				System::Reflection::MemberTypes get_MemberType();
				public:
				Ref<System::String> get_Name();
				public:
				Ref<System::Type> get_DeclaringType();
				public:
				Ref<System::Type> get_ReflectedType();
			};
	}
}
