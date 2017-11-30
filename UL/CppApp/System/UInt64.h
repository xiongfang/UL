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
	struct Byte;
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
	struct Int16;
}
namespace System{
		struct UInt64:public System::ValueType
		{
			public:
			static 			System::UInt64 MaxValue;
			public:
			static 			System::UInt64 MinValue;
			public:
			static System::UInt64 Parse(Ref<System::String>  value);
			public:
			Ref<System::String> ToString();
			public:
			static System::Boolean TryParse(Ref<System::String>  value,System::UInt64 & v);
			public:
			static System::UInt64 op_Addition(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_Substraction(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_Multiply(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_Division(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_Modulus(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_BitwiseAnd(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_BitwiseOr(System::UInt64  a,System::UInt64  b);
			public:
			static System::Boolean op_GreaterThen(System::UInt64  a,System::UInt64  b);
			public:
			static System::Boolean op_LessThen(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_OnesComplement(System::UInt64  a);
			public:
			static System::UInt64 op_LeftShift(System::UInt64  a,System::Int32  b);
			public:
			static System::UInt64 op_RightShift(System::UInt64  a,System::Int32  b);
			public:
			static System::Boolean op_Equality(System::UInt64  a,System::UInt64  b);
			public:
			static System::Boolean op_Inequality(System::UInt64  a,System::UInt64  b);
			public:
			static System::UInt64 op_Increment(System::UInt64  a);
			public:
			static System::UInt64 op_Decrement(System::UInt64  a);
			public:
			static System::UInt64 op_UnaryPlus(System::UInt64  a);
			public:
			static System::Int64 Int64(System::UInt64  v);
			public:
			static System::Int32 Int32(System::UInt64  v);
			public:
			static System::Single Single(System::UInt64  v);
			public:
			static System::Double Double(System::UInt64  v);
			public:
			static System::Byte Byte(System::UInt64  v);
			public:
			static System::SByte SByte(System::UInt64  v);
			public:
			static System::UInt16 UInt16(System::UInt64  v);
			public:
			static System::UInt32 UInt32(System::UInt64  v);
			public:
			static System::Int16 Int16(System::UInt64  v);
		#include "UInt64_ExtHeader.h"
		};
}
