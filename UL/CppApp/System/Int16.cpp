#include "stdafx.h"
#include "System\Int16.h"
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
#include "System\UInt64.h"
#include "System\Exception.h"
System::Int16 System::Int16::MaxValue=0x7FFF;
System::Int16 System::Int16::MinValue=0x8000;
System::Boolean System::Int16::TryParse(Ref<System::String>  value,System::Int16 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::Int16(0);
		return false;
	}
}
