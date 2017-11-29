// CppApp.cpp: 定义控制台应用程序的入口点。
//

#include "stdafx.h"
#include "Console.h"
#include "String.h"
#include "Char.h"
#include "Single.h"
#include "Int32.h"
#include "Boolean.h"
#include "Test.h"
#include <locale.h>

using namespace System;

int main()
{
	_wsetlocale(LC_ALL, L"chs");
	Test::Run();
    return 0;
}

