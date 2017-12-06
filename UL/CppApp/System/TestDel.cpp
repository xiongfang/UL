#include "stdafx.h"
#include "System\TestDel.h"
#include "System\Object.h"
#include "System\Boolean.h"
#include "System\String.h"


System::Boolean System::TestDel::Invoke(Ref<System::String>  v)
{
	return p(v);
}