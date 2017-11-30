#include "stdafx.h"
#include "System\Double.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int64.h"
#include "System\Int32.h"
#include "System\Single.h"
#include "System\Int16.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"
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
