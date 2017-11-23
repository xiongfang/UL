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
		static System::Int64 +(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 -(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 *(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 /(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 %(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 &(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 |(System::Int64  a,System::Int64  b);
		public:
		static System::Int64 ~(System::Int64  a);
		public:
		static System::Int64 <<(System::Int64  a,System::Int32  b);
		public:
		static System::Int64 >>(System::Int64  a,System::Int32  b);
	#include "Int64_ExtHeader.h"
	};
}
