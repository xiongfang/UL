#pragma once
#include "Object.h"
namespace System
{
class String;
}
namespace System
{
struct Char;
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
namespace System{
	class Console:public System::Object
	{
		public:
		static void Write(Ref<System::String>  value);
		public:
		static void Write(Ref<System::Object>  value);
		public:
		static void Write(System::Char  value);
		public:
		static void Write(System::Boolean  value);
		public:
		static void Write(System::Int32  value);
		public:
		static void Write(System::Int64  value);
		public:
		static void Write(System::Single  value);
		public:
		static void Write(System::Double  value);
		public:
		static void Write(System::Byte  value);
		public:
		static void WriteLine();
		public:
		static void WriteLine(System::Char  value);
		public:
		static void WriteLine(System::Boolean  value);
		public:
		static void WriteLine(System::Int32  value);
		public:
		static void WriteLine(System::Int64  value);
		public:
		static void WriteLine(System::Single  value);
		public:
		static void WriteLine(System::Double  value);
		public:
		static void WriteLine(System::Byte  value);
		public:
		static void WriteLine(Ref<System::String>  value);
		public:
		static void WriteLine(Ref<System::Object>  value);
	};
}
