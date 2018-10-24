#pragma once
#include "System\Delegate.h"
#include "System\Boolean.h"
namespace System{
	class String;
}
namespace System{
	class TestDel:public System::Delegate
	{
		public:
		virtual System::Boolean Invoke(Ref<System::String>  v)=0;
	};
	template<typename T>
	class TestDel__Implement:public TestDel
	{
		public:
		typedef System::Boolean(T::*Type)(Ref<System::String>  v);
		typedef TestDel__Implement ThisType;
		Type p;
		typedef System::Boolean(StaticType)(Ref<System::String>  v);
		StaticType* static_p;
		ThisType(T* o, Type p)
		
                                {
	                                _target = o;
	                                this->p = p;
	                                static_p = nullptr;
                                }
		ThisType(T* o, StaticType* p)
		
                                {
	                                _target = o;
	                                this->static_p = p;
	                                p = nullptr;
                                }
		System::Boolean Invoke(Ref<System::String>  v)
		{

			                    for(int i=0;i<list->get_Count()._v;i++)
			                    {
				                    ThisType* thisDel = (ThisType*)list->get_Index(i).Get();
                                			thisDel->Invoke(v);
			}
			if (static_p != nullptr)
			{
				return static_p(v);
			}
			return (((T*)_target.Get())->*p)(v);
		}
	};
}
