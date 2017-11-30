#pragma once
#include "System\Object.h"
namespace System{
	struct Boolean;
}
namespace System{
	namespace Reflection{
		class MemberInfo;
	}
}
namespace System{
	namespace Reflection{
			class MemberFilter:public System::Object
			{
				public:
				System::Boolean Invoke(Ref<System::Reflection::MemberInfo>  m,Ref<System::Object>  filterCriteria);
			};
	}
}
