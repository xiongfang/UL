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
	struct Int32;
}
namespace System{
	struct Int16;
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
	struct Single:public System::ValueType
	{
		public:
static System::Single Epsilon;
		public:
static System::Single MaxValue;
		public:
static System::Single MinValue;
		public:
		static System::Single Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Single & v);
		public:
		static System::Single op_Addition(System::Single  a,System::Single  b);
		public:
		static System::Single op_Substraction(System::Single  a,System::Single  b);
		public:
		static System::Single op_Multiply(System::Single  a,System::Single  b);
		public:
		static System::Single op_Division(System::Single  a,System::Single  b);
		public:
		static System::Boolean op_GreaterThen(System::Single  a,System::Single  b);
		public:
		static System::Boolean op_LessThen(System::Single  a,System::Single  b);
		public:
		static System::Single op_OnesComplement(System::Single  a);
		public:
		static System::Boolean op_Equality(System::Single  a,System::Single  b);
		public:
		static System::Boolean op_Inequality(System::Single  a,System::Single  b);
		public:
		static System::Single op_Increment(System::Single  a);
		public:
		static System::Single op_Decrement(System::Single  a);
		public:
		static System::Single op_UnaryPlus(System::Single  a);
		public:
		static System::Single op_UnaryNegation(System::Single  a);
		public:
		static System::Int64 Int64(System::Single  v);
		public:
		static System::Int32 Int32(System::Single  v);
		public:
		static System::Int16 Int16(System::Single  v);
		public:
		static System::Double Double(System::Single  v);
		public:
		static System::Byte Byte(System::Single  v);
		public:
		static System::SByte SByte(System::Single  v);
		public:
		static System::UInt16 UInt16(System::Single  v);
		public:
		static System::UInt32 UInt32(System::Single  v);
		public:
		static System::UInt64 UInt64(System::Single  v);
	#include "Single_ExtHeader.h"
	};
}
