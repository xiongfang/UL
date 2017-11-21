#include "stdafx.h"
#include "Exception.h"
#include "Object.h"
#include "String.h"
System::Exception::Exception(Ref<System::String>  msg)
{
	_msg = msg;
}
System::Exception::Exception()
{
	_msg = Ref<System::String>(new System::String(""));
}
