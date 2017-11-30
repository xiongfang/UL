#include "stdafx.h"
#include "System\UInt16.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int64.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\Int16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"
System::UInt16 System::UInt16::MaxValue=0xFFFF;
System::UInt16 System::UInt16::MinValue=0;
System::Boolean System::UInt16::TryParse(Ref<System::String>  value,System::UInt16 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::UInt16(0);
		return false;
	}
}
