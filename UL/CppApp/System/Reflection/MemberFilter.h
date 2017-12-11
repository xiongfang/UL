#pragma once
#include "System\Delegate.h"
#include "System\Boolean.h"
namespace System{
	namespace Reflection{
		class MemberInfo;
	}
}
namespace System{
	class Object;
}
namespace System{
	namespace Reflection{
		class MemberFilter:public System::Delegate
		{
			public:
			virtual System::Boolean Invoke(Ref<System::Reflection::MemberInfo>  m,Ref<System::Object>  filterCriteria)=0;
		};
		template<typename T>
		class MemberFilter__Implement:public MemberFilter
		{
			public:
			typedef System::Boolean(T::*Type)(Ref<System::Reflection::MemberInfo>  m,Ref<System::Object>  filterCriteria);
			typedef MemberFilter__Implement ThisType;
			Type p;
			typedef System::Boolean(StaticType)(Ref<System::Reflection::MemberInfo>  m,Ref<System::Object>  filterCriteria);
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
			System::Boolean Invoke(Ref<System::Reflection::MemberInfo>  m,Ref<System::Object>  filterCriteria)
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
