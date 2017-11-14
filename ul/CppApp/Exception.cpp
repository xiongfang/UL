#include "stdafx.h"
#include "Exception.h"
#include "Object.h"
#include "String.h"
System::Exception::Exception(Ref<System::String>  msg)
{
	_msg->op_Assign(Ref<System::Object>(msg.Get()));
}
System::Exception::Exception()
{
	_msg->op_Assign(Ref<System::Object>((new System::String("")).Get()));
}
