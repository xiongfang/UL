#include "stdafx.h"
#include "System\Console.h"
#include "System\Object.h"
#include "System\String.h"
#include "System\Char.h"
#include "System\Boolean.h"
#include "System\Int32.h"
#include "System\Int64.h"
#include "System\Single.h"
#include "System\Double.h"
#include "System\Byte.h"
#include "Console_ExtCpp.h"
void System::Console::Write(Ref<System::Object>  value)
{
	Write(value->ToString());
}
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
	Write(Ref<System::String>(new System::String(_T("\r\n"))));
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
void System::Console::WriteLine(Ref<System::String>  value)
{
	Write(value);
	WriteLine();
}
void System::Console::WriteLine(Ref<System::Object>  value)
{
	Write(value->ToString());
	WriteLine();
}
