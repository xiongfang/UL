#include "stdafx.h"
#include "System\Single.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\Boolean.h"
#include "System\Int64.h"
#include "System\Int32.h"
#include "System\Int16.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "System\SByte.h"
#include "System\UInt16.h"
#include "System\UInt32.h"
#include "System\UInt64.h"
#include "System\Exception.h"
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
