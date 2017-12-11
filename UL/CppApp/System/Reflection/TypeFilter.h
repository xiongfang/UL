#pragma once
#include "System\Delegate.h"
#include "System\Boolean.h"
namespace System{
	class Type;
}
namespace System{
	class Object;
}
namespace System{
	namespace Reflection{
		class TypeFilter:public System::Delegate
		{
			public:
			virtual System::Boolean Invoke(Ref<System::Type>  m,Ref<System::Object>  filterCriteria)=0;
		};
		template<typename T>
		class TypeFilter__Implement:public TypeFilter
		{
			public:
			typedef System::Boolean(T::*Type)(Ref<System::Type>  m,Ref<System::Object>  filterCriteria);
			typedef TypeFilter__Implement ThisType;
			Type p;
			typedef System::Boolean(StaticType)(Ref<System::Type>  m,Ref<System::Object>  filterCriteria);
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
			System::Boolean Invoke(Ref<System::Type>  m,Ref<System::Object>  filterCriteria)
			{

			                    for(int i=0;i<list->get_Count()._v;i++)
			                    {
				                    ThisType* thisDel = (ThisType*)list->get_Index(i).Get();
                                				thisDel->Invoke(m,filterCriteria);
				}
				if (static_p != nullptr)
				{
					return static_p(m,filterCriteria);
				}
				return (((T*)_target.Get())->*p)(v);
			}
		};
	}
}
