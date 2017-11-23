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
		static System::Int32 +(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 -(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 *(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 /(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 %(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 &(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 |(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 ~(System::Int32  a);
		public:
		static System::Int32 <<(System::Int32  a,System::Int32  b);
		public:
		static System::Int32 >>(System::Int32  a,System::Int32  b);
	#include "Int32_ExtHeader.h"
	};
}
