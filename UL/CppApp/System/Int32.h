#pragma once
#include "System\ValueType.h"
namespace System{
	class String;
}
namespace System{
	struct Boolean;
}
namespace System{
	struct Int64;
}
namespace System{
	struct Int16;
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
	struct Int32:public System::ValueType
	{
		public:
static System::Int32 MaxValue;
		public:
static System::Int32 MinValue;
		public:
		static System::Int32 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Int32 & v);
		public:
		static System::Int32 op_Addition(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Substraction(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Multiply(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Division(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Modulus(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_BitwiseAnd(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_BitwiseOr(System::Int32  a,System::Int32  b);
		public:
		static System::Boolean op_GreaterThen(System::Int32  a,System::Int32  b);
		public:
		static System::Boolean op_LessThen(System::Int32  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::Int32  a,System::Int32  b);
		public:
		static System::Boolean op_Inequality(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_LeftShift(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_RightShift(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Increment(System::Int32  a);
		public:
		static System::Int32 op_Decrement(System::Int32  a);
		public:
		static System::Int32 op_OnesComplement(System::Int32  a);
		public:
		static System::Int32 op_UnaryPlus(System::Int32  a);
		public:
		static System::Int32 op_UnaryNegation(System::Int32  a);
		public:
		static System::Int64 Int64(System::Int32  v);
		public:
		static System::Int16 Int16(System::Int32  v);
		public:
		static System::Single Single(System::Int32  v);
		public:
		static System::Double Double(System::Int32  v);
		public:
		static System::Byte Byte(System::Int32  v);
		public:
		static System::SByte SByte(System::Int32  v);
		public:
		static System::UInt16 UInt16(System::Int32  v);
		public:
		static System::UInt32 UInt32(System::Int32  v);
		public:
		static System::UInt64 UInt64(System::Int32  v);
	#include "Int32_ExtHeader.h"
	};
}
