#include "stdafx.h"
#include "Double.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
#include "Int64.h"
#include "Int32.h"
#include "Single.h"
#include "Int16.h"
#include "Byte.h"
#include "SByte.h"
#include "UInt16.h"
#include "UInt32.h"
#include "UInt64.h"
#include "Exception.h"
System::Double System::Double::Epsilon=4.94065645841247e-324;
System::Double System::Double::MaxValue=1.79769313486231e308;
System::Double System::Double::MinValue=-1.79769313486231e308;
System::Boolean System::Double::TryParse(Ref<System::String>  value,System::Double & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::Double(0);
		return false;
	}
}
