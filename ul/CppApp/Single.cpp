#include "stdafx.h"
#include "Single.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Int16.h"
#include "Double.h"
#include "Byte.h"
#include "SByte.h"
#include "UInt16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"
System::Single System::Single::Epsilon=1.4e-45f;
System::Single System::Single::MaxValue=3.40282346e38f;
System::Single System::Single::MinValue=-3.402823e38f;
System::Boolean System::Single::TryParse(Ref<System::String>  value,System::Single & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::Single(0);
		return false;
	}
}
