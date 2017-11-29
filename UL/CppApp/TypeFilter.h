#pragma once
#include "Object.h"
namespace System{
	struct Boolean;
}
namespace System{
	class Type;
}
namespace System{
	namespace Reflection{
			class TypeFilter:public System::Object
			{
				public:
				System::Boolean Invoke(Ref<System::Type>  m,Ref<System::Object>  filterCriteria);
			};
	}
}
