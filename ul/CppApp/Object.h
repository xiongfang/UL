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
		static System::Boolean op_Equality(Ref<System::Object>  a,Ref<System::Object>  b);
		public:
		static System::Boolean op_Inequality(Ref<System::Object>  a,Ref<System::Object>  b);
	};
}
