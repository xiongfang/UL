#include "stdafx.h"
#include "System\Boolean.h"
#include "System\ValueType.h"
#include "System\String.h"
#include "System\ArgumentNullException.h"
#include "System\FormatException.h"
#include "System\Exception.h"

System::Boolean System::Boolean::op_LogicNot(System::Boolean  a)
{
	return !a._v;
}