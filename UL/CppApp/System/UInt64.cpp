#include "stdafx.h"
#include "System\UInt64.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int64.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\Int16.h"
#include "System\Exception.h"
System::UInt64 System::UInt64::MaxValue=0xFFFFFFFFFFFFFFFF;
System::UInt64 System::UInt64::MinValue=0;
System::Boolean System::UInt64::TryParse(Ref<System::String>  value,System::UInt64 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::UInt64(0);
		return false;
	}
}
