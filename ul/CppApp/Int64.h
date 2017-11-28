#pragma once
#include "Object.h"
namespace System
{
class String;
}
namespace System
{
struct Boolean;
}
namespace System
{
struct Int32;
}
namespace System
{
struct Int16;
}
namespace System
{
struct Single;
}
namespace System
{
struct Double;
}
namespace System
{
struct Byte;
}
namespace System
{
struct SByte;
}
namespace System
{
struct UInt16;
}
namespace System
{
struct UInt32;
}
namespace System
{
struct UInt64;
}
namespace System{
	struct Int64:public System::Object
	{
		public:
		static 		System::Int64 MaxValue;
		public:
		static 		System::Int64 MinValue;
		public:
		static System::Int64 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Int64 & v);
		public:
		static System::Int64 op_Addition(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_Substraction(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_Multiply(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_Division(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_Modulus(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_BitwiseAnd(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_BitwiseOr(System::Int64  a,System::Int64  b);
		public:
		static System::Boolean op_GreaterThen(System::Int64  a,System::Int64  b);
		public:
		static System::Boolean op_LessThen(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_OnesComplement(System::Int64  a);
		public:
		static System::Int64 op_LeftShift(System::Int64  a,System::Int32  b);
		public:
		static System::Int64 op_RightShift(System::Int64  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::Int64  a,System::Int64  b);
		public:
		static System::Boolean op_Inequality(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 op_Increment(System::Int64  a);
		public:
		static System::Int64 op_Decrement(System::Int64  a);
		public:
		static System::Int64 op_UnaryPlus(System::Int64  a);
		public:
		static System::Int64 op_UnaryNegation(System::Int64  a);
		public:
		static System::Int16 Int16(System::Int64  v);
		public:
		static System::Int32 Int32(System::Int64  v);
		public:
		static System::Single Single(System::Int64  v);
		public:
		static System::Double Double(System::Int64  v);
		public:
		static System::Byte Byte(System::Int64  v);
		public:
		static System::SByte SByte(System::Int64  v);
		public:
		static System::UInt16 UInt16(System::Int64  v);
		public:
		static System::UInt32 UInt32(System::Int64  v);
		public:
		static System::UInt64 UInt64(System::Int64  v);
	#include "Int64_ExtHeader.h"
	};
}
