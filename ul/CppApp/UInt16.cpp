#include "stdafx.h"
#include "UInt16.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "SByte.h"
#include "Int16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"
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
