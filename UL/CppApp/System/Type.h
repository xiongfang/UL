#pragma once
#include "System\Object.h"
namespace System{
	class String;
}
namespace System{
	struct Boolean;
}
#include "System\t_ArrayT.h"
namespace System{
	namespace Reflection{
		class TypeFilter;
	}
}
#include "System\Reflection\MemberInfo.h"
namespace System{
	namespace Reflection{
		struct MemberTypes;
	}
}
namespace System{
	namespace Reflection{
		struct BindingFlags;
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
		virtual Ref<System::Type> get_BaseType()=0;
		public:
		virtual Ref<System::Type> get_DeclaringType()=0;
		public:
		virtual Ref<System::String> get_FullName()=0;
		public:
		virtual System::Boolean get_IsAbstract()=0;
		public:
		virtual System::Boolean get_IsClass()=0;
		public:
		virtual System::Boolean get_IsEnum()=0;
		public:
		virtual System::Boolean get_IsGenericType()=0;
		public:
		virtual System::Boolean get_IsInterface()=0;
		public:
		virtual System::Boolean get_IsPublic()=0;
		public:
		virtual System::Boolean get_IsValueType()=0;
		public:
		virtual Ref<System::String> get_Name()=0;
		public:
		virtual Ref<System::String> get_Namespace()=0;
		public:
		virtual Ref<System::ArrayT<System::Type>> FindInterfaces(Ref<System::Reflection::TypeFilter>  filter,Ref<System::Object>  filterCriteria)=0;
		public:
		virtual Ref<System::ArrayT<System::Reflection::MemberInfo>> FindMembers(System::Reflection::MemberTypes  memberType,System::Reflection::BindingFlags  bindingAttr,Ref<System::Reflection::MemberFilter>  filter,Ref<System::Object>  filterCriteria)=0;
	};
}
