#pragma once
#include "System\ValueType.h"
namespace System{
	struct Boolean;
}
namespace System{
	class Object;
}
namespace System{
	class Array;
}
namespace System{
	class Type;
}
namespace System{
	class Enum:public System::ValueType
	{
		public:
		System::Boolean Equals(Ref<System::Object>  obj);
		public:
		static Ref<System::Array> GetValues(Ref<System::Type>  enumType);
		public:
		static Ref<System::Enum> op_BitwiseAnd(Ref<System::Enum>  a,Ref<System::Enum>  b);
		public:
		static Ref<System::Enum> op_BitwiseOr(Ref<System::Enum>  a,Ref<System::Enum>  b);
		public:
		static Ref<System::Enum> op_OnesComplement(Ref<System::Enum>  a);
		public:
		static System::Boolean op_Equality(Ref<System::Enum>  a,Ref<System::Enum>  b);
		public:
		static System::Boolean op_Inequality(Ref<System::Enum>  a,Ref<System::Enum>  b);
	#include "Enum_Ext.h"
	};
}
