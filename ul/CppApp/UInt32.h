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
struct UInt16;
}
namespace System
{
struct UInt64;
}
namespace System
{
struct Int16;
}
namespace System{
	struct UInt32:public System::Object
	{
		public:
		static 		System::UInt32 MaxValue;
		public:
		static 		System::UInt32 MinValue;
		public:
		static System::UInt32 Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::UInt32 & v);
		public:
		static System::UInt32 op_Addition(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_Substraction(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_Multiply(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_Division(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_Modulus(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_BitwiseAnd(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_BitwiseOr(System::UInt32  a,System::UInt32  b);
		public:
		static System::Boolean op_GreaterThen(System::UInt32  a,System::UInt32  b);
		public:
		static System::Boolean op_LessThen(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_OnesComplement(System::UInt32  a);
		public:
		static System::UInt32 op_LeftShift(System::UInt32  a,System::Int32  b);
		public:
		static System::UInt32 op_RightShift(System::UInt32  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::UInt32  a,System::UInt32  b);
		public:
		static System::Boolean op_Inequality(System::UInt32  a,System::UInt32  b);
		public:
		static System::UInt32 op_Increment(System::UInt32  a);
		public:
		static System::UInt32 op_Decrement(System::UInt32  a);
		public:
		static System::Int64 Int64(System::UInt32  v);
		public:
		static System::Int32 Int32(System::UInt32  v);
		public:
		static System::Single Single(System::UInt32  v);
		public:
		static System::Double Double(System::UInt32  v);
		public:
		static System::Byte Byte(System::UInt32  v);
		public:
		static System::SByte SByte(System::UInt32  v);
		public:
		static System::UInt16 UInt16(System::UInt32  v);
		public:
		static System::UInt64 UInt64(System::UInt32  v);
		public:
		static System::Int16 Int16(System::UInt32  v);
	#include "UInt32_ExtHeader.h"
	};
}
