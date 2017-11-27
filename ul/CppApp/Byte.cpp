#include "stdafx.h"
#include "Byte.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Int16.h"
#include "SByte.h"
#include "UInt16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"
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
