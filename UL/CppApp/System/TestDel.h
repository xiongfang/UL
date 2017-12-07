#pragma once
#include "System\Object.h"
#include "System\Boolean.h"
namespace System{
	class String;
}
namespace System{
		class TestDel:public System::Object
		{
			public:
			virtual System::Boolean Invoke(Ref<System::String>  v)=0;
		};
	template<typename T>
	class TestDel__Implement:public TestDel
	{
		public:
		typedef System::Boolean(T::*Type)(Ref<System::String>  v);
		Ref<T> object;
		Type p;
		typedef System::Boolean(StaticType)(Ref<System::String>  v);
		StaticType* static_p;
		TestDel__Implement(T* o, Type p)
		
                                {
	                                object = o;
	                                this->p = p;
	                                static_p = nullptr;
                                }
		TestDel__Implement(T* o, StaticType* p)
		
                                {
	                                object = o;
	                                this->static_p = p;
	                                p = nullptr;
                                }
		System::Boolean Invoke(Ref<System::String>  v)
		{
			if (static_p != nullptr)
			{
				return static_p(v);
			}
			return (object.Get()->*p)(v);
		}
	};
}
