#include "stdafx.h"
#include "UInt64.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "SByte.h"
#include "UInt16.h"
#include "UInt32.h"
#include "Int16.h"
#include "Exception.h"
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
