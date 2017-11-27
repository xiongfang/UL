#include "stdafx.h"
#include "Boolean.h"
#include "Object.h"
#include "String.h"
#include "ArgumentNullException.h"
#include "FormatException.h"
#include "Exception.h"

System::Boolean System::Boolean::op_LogicNot(System::Boolean  a)
{
	return !a._v;
}