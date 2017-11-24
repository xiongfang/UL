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
struct Int64;
}
namespace System{
	struct Int32:public System::Object
	{
		public:
		static 		System::Int32 MaxValue;
		public:
		static 		System::Int32 MinValue;
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
		static System::Int32 op_OnesComplement(System::Int32  a);
		public:
		static System::Int32 op_LeftShift(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_RightShift(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Equality(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 op_Inequality(System::Int32  a,System::Int32  b);
		public:
		static System::Int64 Int64(System::Int32  v);
	#include "Int32_ExtHeader.h"
	};
}
