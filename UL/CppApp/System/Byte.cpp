#include "stdafx.h"
#include "System\Byte.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int64.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Int16.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"
System::Byte System::Byte::MaxValue=255;
System::Byte System::Byte::MinValue=0;
System::Boolean System::Byte::TryParse(Ref<System::String>  value,System::Byte & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::Byte(0);
		return false;
	}
}
