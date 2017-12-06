#pragma once
#include "System\Object.h"
#include "System\Boolean.h"
namespace System{
	struct Boolean;
}
namespace System{
	class String;
}
namespace System{
		template<typename T>
		class TestDel:public System::Object
		{
			public:
				System::Boolean Invoke(Ref<System::String>  v)
				{
					return (*this)(v);
				}

				typedef System::Boolean(T::*Type)(Ref<System::String>  v);
				Ref<T> object;
				Type p;

				typedef System::Boolean(StaticType)(Ref<System::String>  v);

				StaticType* static_p;

				TestDel(T* o, Type p)
				{
					object = o;
					this->p = p;
					static_p = nullptr;
				}
				TestDel(T* o, StaticType* p)
				{
					object = o;
					this->static_p = p;
					p = nullptr;
				}

				System::Boolean operator()(Ref<System::String>  v)
				{
					if (static_p != nullptr)
					{
						return static_p(v);
					}

					return (object.Get()->*p)(v);
				}
		};
}
