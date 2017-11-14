#pragma once
namespace System
{
class Type;
}
namespace System
{
struct Boolean;
}
namespace System
{
class String;
}
namespace System{
	class Object	{
		public:
		virtual Ref<System::Type> GetType();
		public:
		virtual System::Boolean Equals(Ref<System::Object>  v);
		public:
		virtual Ref<System::String> ToString();
		public:
		virtual System::Boolean op_Equals(Ref<System::Object>  b);
		public:
		virtual Ref<System::Object> op_Assign(Ref<System::Object>  b);
	};
}
