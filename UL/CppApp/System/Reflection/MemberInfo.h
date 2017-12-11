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
			virtual System::Reflection::MemberTypes get_MemberType()=0;
			public:
			virtual Ref<System::String> get_Name()=0;
			public:
			virtual Ref<System::Type> get_DeclaringType()=0;
			public:
			virtual Ref<System::Type> get_ReflectedType()=0;
		};
	}
}
