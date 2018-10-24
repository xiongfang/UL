#include "stdafx.h"
#include "System\Int32.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int64.h"
#include "System\Int16.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"
#include "Int32_ExtCpp.h"
System::Int32 System::Int32::MaxValue=0x7FFFFFFF;
System::Int32 System::Int32::MinValue=0x80000000;
System::Boolean System::Int32::TryParse(Ref<System::String>  value,System::Int32 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = 0;
		return false;
	}
}
