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
struct Int16;
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
	struct Double:public System::Object
	{
		public:
		static 		System::Double Epsilon;
		public:
		static 		System::Double MaxValue;
		public:
		static 		System::Double MinValue;
		public:
		static System::Double Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Double & v);
		public:
		static System::Double op_Addition(System::Double  a,System::Double  b);
		public:
		static System::Double op_Substraction(System::Double  a,System::Double  b);
		public:
		static System::Double op_Multiply(System::Double  a,System::Double  b);
		public:
		static System::Double op_Division(System::Double  a,System::Double  b);
		public:
		static System::Double op_Modulus(System::Double  a,System::Double  b);
		public:
		static System::Double op_BitwiseAnd(System::Double  a,System::Double  b);
		public:
		static System::Double op_BitwiseOr(System::Double  a,System::Double  b);
		public:
		static System::Boolean op_GreaterThen(System::Double  a,System::Double  b);
		public:
		static System::Boolean op_LessThen(System::Double  a,System::Double  b);
		public:
		static System::Double op_OnesComplement(System::Double  a);
		public:
		static System::Double op_LeftShift(System::Double  a,System::Int32  b);
		public:
		static System::Double op_RightShift(System::Double  a,System::Int32  b);
		public:
		static System::Boolean op_Equality(System::Double  a,System::Double  b);
		public:
		static System::Boolean op_Inequality(System::Double  a,System::Double  b);
		public:
		static System::Double op_Increment(System::Double  a);
		public:
		static System::Double op_Decrement(System::Double  a);
		public:
		static System::Int64 Int64(System::Double  v);
		public:
		static System::Int32 Int32(System::Double  v);
		public:
		static System::Single Single(System::Double  v);
		public:
		static System::Int16 Int16(System::Double  v);
		public:
		static System::Byte Byte(System::Double  v);
		public:
		static System::SByte SByte(System::Double  v);
		public:
		static System::UInt16 UInt16(System::Double  v);
		public:
		static System::UInt32 UInt32(System::Double  v);
		public:
		static System::UInt64 UInt64(System::Double  v);
	#include "Double_ExtHeader.h"
	};
}
