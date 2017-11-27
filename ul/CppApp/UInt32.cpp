﻿#include "stdafx.h"
#include "UInt32.h"
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
#include "UInt64.h"
#include "Int16.h"
#include "Exception.h"
System::UInt32 System::UInt32::MaxValue=0xFFFFFFFF;
System::UInt32 System::UInt32::MinValue=0;
System::Boolean System::UInt32::TryParse(Ref<System::String>  value,System::UInt32 & v)
{
	try
	{
		v = Parse(value);
		return true;
	}
	catch(System::Exception e)
	{
		v = System::Int32::UInt32(0);
		return false;
	}
}
