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
struct Int16;
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
	struct SByte:public System::Object
	{
		public:
		static 		System::SByte MaxValue;
		public:
		static 		System::SByte MinValue;
		public:
		static System::SByte Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::SByte & v);
		public:
		static System::SByte op_Addition(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_Substraction(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_Multiply(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_Division(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_Modulus(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_BitwiseAnd(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_BitwiseOr(System::SByte  a,System::SByte  b);
		public:
		static System::Boolean op_GreaterThen(System::SByte  a,System::SByte  b);
		public:
		static System::Boolean op_LessThen(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_OnesComplement(System::SByte  a);
		public:
		static System::SByte op_LeftShift(System::SByte  a,System::Int32  b);
		public:
		static System::SByte op_RightShift(System::SByte  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::SByte  a,System::SByte  b);
		public:
		static System::Boolean op_Inequality(System::SByte  a,System::SByte  b);
		public:
		static System::SByte op_Increment(System::SByte  a);
		public:
		static System::SByte op_Decrement(System::SByte  a);
		public:
		static System::Int64 Int64(System::SByte  v);
		public:
		static System::Int32 Int32(System::SByte  v);
		public:
		static System::Single Single(System::SByte  v);
		public:
		static System::Double Double(System::SByte  v);
		public:
		static System::Byte Byte(System::SByte  v);
		public:
		static System::Int16 Int16(System::SByte  v);
		public:
		static System::UInt16 UInt16(System::SByte  v);
		public:
		static System::UInt32 UInt32(System::SByte  v);
		public:
		static System::UInt64 UInt64(System::SByte  v);
	#include "SByte_ExtHeader.h"
	};
}
