#pragma once
#include "Object.h"
namespace System{
	class String;
}
namespace System{
	struct Boolean;
}
#include "Array.h"
namespace System{
	namespace Reflection{
		class TypeFilter;
	}
}
#include "MemberInfo.h"
namespace System{
	namespace Reflection{
		enum MemberTypes;
	}
}
namespace System{
	namespace Reflection{
		enum BindingFlags;
	}
}
namespace System{
	namespace Reflection{
		class MemberFilter;
	}
}
namespace System{
		class Type:public System::Object
		{
			public:
			Ref<System::Type> get_BaseType();
			public:
			Ref<System::Type> get_DeclaringType();
			public:
			Ref<System::String> get_FullName();
			public:
			System::Boolean get_IsAbstract();
			public:
			System::Boolean get_IsClass();
			public:
			System::Boolean get_IsEnum();
			public:
			System::Boolean get_IsGenericType();
			public:
			System::Boolean get_IsInterface();
			public:
			System::Boolean get_IsPublic();
			public:
			System::Boolean get_IsValueType();
			public:
			Ref<System::String> get_Name();
			public:
			Ref<System::String> get_Namespace();
			public:
			virtual Ref<System::Array<System::Type>> FindInterfaces(Ref<System::Reflection::TypeFilter>  filter,Ref<System::Object>  filterCriteria);
			public:
			virtual Ref<System::Array<System::Reflection::MemberInfo>> FindMembers(System::Reflection::MemberTypes  memberType,System::Reflection::BindingFlags  bindingAttr,Ref<System::Reflection::MemberFilter>  filter,Ref<System::Object>  filterCriteria);
		};
}
