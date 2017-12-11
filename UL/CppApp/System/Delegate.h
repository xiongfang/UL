#pragma once
#include "System\Object.h"
#include "System\Collections\Generic\t_List.h"
#include "System\t_ArrayT.h"
namespace System{
	struct Boolean;
}
namespace System{
	class Delegate:public System::Object
	{
		protected:
Ref<System::Object> _target;
		protected:
Ref<System::Collections::Generic::List<System::Delegate>> list;
		public:
		Ref<System::Object> get_Target();
		public:
		Delegate();
		public:
		static Ref<System::Delegate> Combine(Ref<System::Delegate>  a,Ref<System::Delegate>  b);
		public:
		static Ref<System::Delegate> Combine(Ref<System::ArrayT<System::Delegate>>  delegates);
		public:
		static Ref<System::Delegate> Remove(Ref<System::Delegate>  source,Ref<System::Delegate>  value);
		public:
		static Ref<System::Delegate> RemoveAll(Ref<System::Delegate>  source,Ref<System::Delegate>  value);
		public:
		Ref<System::ArrayT<System::Delegate>> GetInvocationList();
		protected:
		Ref<System::Delegate> CombineImpl(Ref<System::Delegate>  d);
		public:
		static System::Boolean op_Equality(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2);
		public:
		static System::Boolean op_Inequality(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2);
		public:
		static Ref<System::Delegate> op_Addition(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2);
		public:
		static Ref<System::Delegate> op_Substraction(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2);
	};
}
