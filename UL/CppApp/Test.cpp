#include "stdafx.h"
#include "Test.h"
#include "Object.h"
#include "Int32.h"
#include "Console.h"
void System::Test::Run()
{
	System::Int32 a=5;
	a = System::Int32::op_Addition(a,6);
	System::Console::WriteLine(a);
	a = System::Int32::op_Addition(a,7);
	System::Console::WriteLine(a);
}
