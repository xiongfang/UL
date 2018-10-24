#include "stdafx.h"
#include "System\Char.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Exception.h"
System::Char System::Char::MaxValue=0xFFFF;
System::Char System::Char::MinValue=0;
System::Boolean System::Char::TryParse(Ref<System::String>  value,System::Char & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = _T('\0');
		return false;
	}
}
