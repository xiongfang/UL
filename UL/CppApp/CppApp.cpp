// CppApp.cpp: 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "Console.h"
#include "String.h"
#include "Char.h"
#include "Single.h"
#include "Int32.h"
#include "Boolean.h"

using namespace System;

int main()
{
	Char v = _T('你');
	String hv = "你好";
	System::Console::WriteLine(Ref<String>(&hv));

	System::Console::WriteLine(v);

	System::Console::WriteLine(Ref<System::String>(new System::String("hello world!")));


	Int32 k = 7;
	System::Console::WriteLine(k);

	String stringInt = "666";
	Int32 k2;
	if (Int32::TryParse(&stringInt,k2))
	{
		System::Console::WriteLine(k2);
	}

    return 0;
}

