#include "stdafx.h"
#include "System\Delegate.h"
#include "System\Object.h"
#include "System\Collections\Generic\t_List.h"
#include "System\t_ArrayT.h"
#include "System\Boolean.h"
#include "System\Int32.h"
using namespace System::Collections::Generic;
Ref<System::Object> System::Delegate::get_Target()
{
	return _target;
}
System::Delegate::Delegate()
{
	list = new System::Collections::Generic::List<System::Delegate>();
}
Ref<System::Delegate> System::Delegate::Combine(Ref<System::Delegate>  a,Ref<System::Delegate>  b)
{
	if(System::Delegate::op_Equality(Ref<System::Object>(a),nullptr))
		return b;
	if(System::Delegate::op_Equality(Ref<System::Object>(b),nullptr))
		return a;
	return a->CombineImpl(b);
}
Ref<System::Delegate> System::Delegate::Combine(Ref<System::ArrayT<System::Delegate>>  delegates)
{
	Ref<System::Delegate> left = nullptr;
	for(	System::Int32 i = 0;System::Int32::op_LessThen(i,delegates->get_Length());PostfixUnaryHelper::op_Increment<System::Int32>(i))
	{
		left = Combine(left,delegates->get_Index(i));
	}
	return left;
}
Ref<System::Delegate> System::Delegate::Remove(Ref<System::Delegate>  source,Ref<System::Delegate>  value)
{
	if(System::Delegate::op_Equality(Ref<System::Object>(source),nullptr))
		return nullptr;
	if(System::Delegate::op_Equality(Ref<System::Object>(value),nullptr))
		return source;
	source->list->Remove(value);
	return source;
}
Ref<System::Delegate> System::Delegate::RemoveAll(Ref<System::Delegate>  source,Ref<System::Delegate>  value)
{
	if(System::Delegate::op_Equality(Ref<System::Object>(source),nullptr))
		return nullptr;
	if(System::Delegate::op_Equality(Ref<System::Object>(value),nullptr))
		return source;
	source->list->RemoveAll(value);
	return source;
}
Ref<System::ArrayT<System::Delegate>> System::Delegate::GetInvocationList()
{
	return list->ToArray();
}
Ref<System::Delegate> System::Delegate::CombineImpl(Ref<System::Delegate>  d)
{
	list->Add(d);
	return this;
}
System::Boolean System::Delegate::op_Equality(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2)
{
	if(ReferenceEquals(Ref<System::Object>(d1.Get()),Ref<System::Object>(d2.Get())))
		return true;
	if(ReferenceEquals(Ref<System::Object>(d1.Get()),nullptr))
		return false;
	if(ReferenceEquals(Ref<System::Object>(d2.Get()),nullptr))
		return false;
	return d1->Equals(Ref<System::Object>(d2.Get()));
}
System::Boolean System::Delegate::op_Inequality(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2)
{
	return System::Boolean::op_LogicNot((System::Delegate::op_Equality(d1,d2)));
}
Ref<System::Delegate> System::Delegate::op_Addition(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2)
{
	return Combine(d1,d2);
}
Ref<System::Delegate> System::Delegate::op_Substraction(Ref<System::Delegate>  d1,Ref<System::Delegate>  d2)
{
	return Remove(d1,d2);
}
