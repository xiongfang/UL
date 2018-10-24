#include "stdafx.h"
#include "System\Object.h"
#include "System\Type.h"
#include "System\Boolean.h"
#include "System\String.h"
#include "Object_ExtCpp.h"
System::Boolean System::Object::op_Inequality(Ref<System::Object>  a,Ref<System::Object>  b)
{
	return System::Boolean::op_LogicNot((System::Object::op_Equality(a,b)));
}
