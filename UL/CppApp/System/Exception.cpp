#include "stdafx.h"
#include "System\Exception.h"
#include "System\Object.h"
#include "System\String.h"
System::Exception::Exception(Ref<System::String>  msg)
{
	_msg = msg;
}
System::Exception::Exception()
{
	_msg = Ref<System::String>(new System::String(_T("")));
}
