#pragma once
#include "System\ValueType.h"
namespace System{
	class String;
}
namespace System{
	struct Boolean;
}
namespace System{
	struct Int32;
}
namespace System{
	struct Int64;
}
namespace System{
	struct Single;
}
namespace System{
	struct Double;
}
namespace System{
	struct Int16;
}
namespace System{
	struct SByte;
}
namespace System{
	struct UInt16;
}
namespace System{
	struct UInt32;
}
namespace System{
	struct UInt64;
}
namespace System{
	struct Byte:public System::ValueType
	{
		public:
		static 		System::Byte MaxValue;
		public:
		static 		System::Byte MinValue;
		public:
		static System::Byte Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Byte & v);
		public:
		static System::Byte op_Addition(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_Substraction(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_Multiply(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_Division(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_Modulus(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_BitwiseAnd(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_BitwiseOr(System::Byte  a,System::Byte  b);
		public:
		static System::Boolean op_GreaterThen(System::Byte  a,System::Byte  b);
		public:
		static System::Boolean op_LessThen(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_OnesComplement(System::Byte  a);
		public:
		static System::Byte op_LeftShift(System::Byte  a,System::Int32  b);
		public:
		static System::Byte op_RightShift(System::Byte  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::Byte  a,System::Byte  b);
		public:
		static System::Boolean op_Inequality(System::Byte  a,System::Byte  b);
		public:
		static System::Byte op_Increment(System::Byte  a);
		public:
		static System::Byte op_Decrement(System::Byte  a);
		public:
		static System::Byte op_UnaryPlus(System::Byte  a);
		public:
		static System::Byte op_UnaryNegation(System::Byte  a);
		public:
		static System::Int64 Int64(System::Byte  v);
		public:
		static System::Int32 Int32(System::Byte  v);
		public:
		static System::Single Single(System::Byte  v);
		public:
		static System::Double Double(System::Byte  v);
		public:
		static System::Int16 Int16(System::Byte  v);
		public:
		static System::SByte SByte(System::Byte  v);
		public:
		static System::UInt16 UInt16(System::Byte  v);
		public:
		static System::UInt32 UInt32(System::Byte  v);
		public:
		static System::UInt64 UInt64(System::Byte  v);
	#include "Byte_ExtHeader.h"
	};
}
