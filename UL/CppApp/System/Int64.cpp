#include "stdafx.h"
#include "System\Int64.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int16.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"
System::Int64 System::Int64::MaxValue=0x7FFFFFFFFFFFFFFF;
System::Int64 System::Int64::MinValue=0x8000000000000000;
System::Boolean System::Int64::TryParse(Ref<System::String>  value,System::Int64 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::Int64(0);
		return false;
	}
}
