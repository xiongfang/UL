// CppApp.cpp: 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "Console.h"
#include "String.h"
#include "Char.h"
#include "Single.h"
#include "Int32.h"

using namespace System;

int main()
{
	Char v = _T('你');
	String hv = "你好";
	System::Console::WriteLine(Ref<String>(&hv));

	System::Console::WriteLine(v);

	System::Console::WriteLine(Ref<System::String>(new System::String("hello world!")));
    return 0;
}

