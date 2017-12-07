#pragma once
#include "System\Object.h"
#include "System\Boolean.h"
namespace System{
	class Type;
}
namespace System{
	namespace Reflection{
		class TypeFilter:public System::Object
		{
			public:
			virtual System::Boolean Invoke(Ref<System::Type>  m,Ref<System::Object>  filterCriteria)=0;
		};
		template<typename T>
		class TypeFilter__Implement:public TypeFilter
		{
			public:
			typedef System::Boolean(T::*Type)(Ref<System::Type>  m,Ref<System::Object>  filterCriteria);
			Ref<T> object;
			Type p;
			typedef System::Boolean(StaticType)(Ref<System::Type>  m,Ref<System::Object>  filterCriteria);
			StaticType* static_p;
			TypeFilter__Implement(T* o, Type p)
			
                                {
	                                object = o;
	                                this->p = p;
	                                static_p = nullptr;
                                }
			TypeFilter__Implement(T* o, StaticType* p)
			
                                {
	                                object = o;
	                                this->static_p = p;
	                                p = nullptr;
                                }
			System::Boolean Invoke(Ref<System::Type>  m,Ref<System::Object>  filterCriteria)
			{
				if (static_p != nullptr)
				{
					return static_p(m,filterCriteria);
				}
				return (object.Get()->*p)(v);
			}
		};
	}
}
