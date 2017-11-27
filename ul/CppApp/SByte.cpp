#include "stdafx.h"
#include "SByte.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "Int16.h"
#include "UInt16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"
System::SByte System::SByte::MaxValue=127;
System::SByte System::SByte::MinValue=-128;
System::Boolean System::SByte::TryParse(Ref<System::String>  value,System::SByte & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::SByte(0);
		return false;
	}
}
