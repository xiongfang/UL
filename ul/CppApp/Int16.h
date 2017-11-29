#pragma once
#include "Object.h"
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
	struct UInt64;
}
namespace System{
		struct Int16:public System::Object
		{
			public:
			static 			System::Int16 MaxValue;
			public:
			static 			System::Int16 MinValue;
			public:
			static System::Int16 Parse(Ref<System::String>  value);
			public:
			Ref<System::String> ToString();
			public:
			static System::Boolean TryParse(Ref<System::String>  value,System::Int16 & v);
			public:
			static System::Int16 op_Addition(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_Substraction(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_Multiply(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_Division(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_Modulus(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_BitwiseAnd(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_BitwiseOr(System::Int16  a,System::Int16  b);
			public:
			static System::Boolean op_GreaterThen(System::Int16  a,System::Int16  b);
			public:
			static System::Boolean op_LessThen(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_OnesComplement(System::Int16  a);
			public:
			static System::Int16 op_LeftShift(System::Int16  a,System::Int32  b);
			public:
			static System::Int16 op_RightShift(System::Int16  a,System::Int32  b);
			public:
			static System::Boolean op_Equality(System::Int16  a,System::Int16  b);
			public:
			static System::Boolean op_Inequality(System::Int16  a,System::Int16  b);
			public:
			static System::Int16 op_Increment(System::Int16  a);
			public:
			static System::Int16 op_Decrement(System::Int16  a);
			public:
			static System::Int64 Int64(System::Int16  v);
			public:
			static System::Int32 Int32(System::Int16  v);
			public:
			static System::Single Single(System::Int16  v);
			public:
			static System::Double Double(System::Int16  v);
			public:
			static System::Byte Byte(System::Int16  v);
			public:
			static System::SByte SByte(System::Int16  v);
			public:
			static System::UInt16 UInt16(System::Int16  v);
			public:
			static System::UInt32 UInt32(System::Int16  v);
			public:
			static System::UInt64 UInt64(System::Int16  v);
		#include "Int16_ExtHeader.h"
		};
}
