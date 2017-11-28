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
struct Int64;
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
struct Int16;
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
	struct UInt16:public System::Object
	{
		public:
		static 		System::UInt16 MaxValue;
		public:
		static 		System::UInt16 MinValue;
		public:
		static System::UInt16 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::UInt16 & v);
		public:
		static System::UInt16 op_Addition(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_Substraction(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_Multiply(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_Division(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_Modulus(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_BitwiseAnd(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_BitwiseOr(System::UInt16  a,System::UInt16  b);
		public:
		static System::Boolean op_GreaterThen(System::UInt16  a,System::UInt16  b);
		public:
		static System::Boolean op_LessThen(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_OnesComplement(System::UInt16  a);
		public:
		static System::UInt16 op_LeftShift(System::UInt16  a,System::Int32  b);
		public:
		static System::UInt16 op_RightShift(System::UInt16  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::UInt16  a,System::UInt16  b);
		public:
		static System::Boolean op_Inequality(System::UInt16  a,System::UInt16  b);
		public:
		static System::UInt16 op_Increment(System::UInt16  a);
		public:
		static System::UInt16 op_Decrement(System::UInt16  a);
		public:
		static System::UInt16 op_UnaryPlus(System::UInt16  a);
		public:
		static System::Int64 Int64(System::UInt16  v);
		public:
		static System::Int32 Int32(System::UInt16  v);
		public:
		static System::Single Single(System::UInt16  v);
		public:
		static System::Double Double(System::UInt16  v);
		public:
		static System::Byte Byte(System::UInt16  v);
		public:
		static System::SByte SByte(System::UInt16  v);
		public:
		static System::Int16 Int16(System::UInt16  v);
		public:
		static System::UInt32 UInt32(System::UInt16  v);
		public:
		static System::UInt64 UInt64(System::UInt16  v);
	#include "UInt16_ExtHeader.h"
	};
}
