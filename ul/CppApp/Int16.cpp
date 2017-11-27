#include "stdafx.h"
#include "Int16.h"
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
#include "UInt64.h"
#include "Exception.h"
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
