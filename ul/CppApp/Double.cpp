#include "stdafx.h"
#include "Double.h"
#include "Object.h"
#include "String.h"
#include "Boolean.h"
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
		v = 0;
		return false;
	}
}
