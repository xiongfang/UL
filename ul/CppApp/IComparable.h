#pragma once
namespace System
{
struct Int32;
}
namespace System
{
class Object;
}
namespace System{
	class IComparable	{
		public:
		System::Int32 CompareTo(Ref<System::Object>  obj);
	};
}
