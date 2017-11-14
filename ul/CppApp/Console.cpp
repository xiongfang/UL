#include "stdafx.h"
#include "Console.h"
#include "Object.h"
#include "String.h"
#include "Char.h"
#include "Boolean.h"
#include "Int32.h"
#include "Int64.h"
#include "Single.h"
#include "Double.h"
#include "Byte.h"
#include "Console_ExtCpp.h"
void System::Console::Write(System::Char  value)
{
	Write(value.ToString());
}
void System::Console::Write(System::Boolean  value)
{
	Write(value.ToString());
}
void System::Console::Write(System::Int32  value)
{
	Write(value.ToString());
}
void System::Console::Write(System::Int64  value)
{
	Write(value.ToString());
}
void System::Console::Write(System::Single  value)
{
	Write(value.ToString());
}
void System::Console::Write(System::Double  value)
{
	Write(value.ToString());
}
void System::Console::Write(System::Byte  value)
{
	Write(value.ToString());
}
void System::Console::WriteLine()
{
	Write((new System::String("\r\n")));
}
void System::Console::WriteLine(System::Char  value)
{
	Write(value.ToString());
	WriteLine();
}
void System::Console::WriteLine(System::Boolean  value)
{
	Write(value.ToString());
	WriteLine();
}
void System::Console::WriteLine(System::Int32  value)
{
	Write(value.ToString());
	WriteLine();
}
void System::Console::WriteLine(System::Int64  value)
{
	Write(value.ToString());
	WriteLine();
}
void System::Console::WriteLine(System::Single  value)
{
	Write(value.ToString());
	WriteLine();
}
void System::Console::WriteLine(System::Double  value)
{
	Write(value.ToString());
	WriteLine();
}
void System::Console::WriteLine(System::Byte  value)
{
	Write(value.ToString());
	WriteLine();
}
