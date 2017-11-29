#include "stdafx.h"
#include "Boolean.h"
#include "Object.h"
#include "String.h"
#include "Byte.h"
#include "Single.h"
#include "SByte.h"
#include "Int16.h"
#include "UInt16.h"
#include "Int32.h"
#include "UInt32.h"
#include "Int64.h"
#include "UInt64.h"
#include "Double.h"
#include "Char.h"

using namespace System;


Ref<System::String> Boolean::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> Byte::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> SByte::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> Int16::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> UInt16::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> Int32::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> UInt32::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%d", _v);

	return new String(v);
}

Ref<System::String> Int64::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%lld", _v);

	return new String(v);
}

Ref<System::String> UInt64::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%lld", _v);

	return new String(v);
}

Ref<System::String> Single::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%f", _v);

	return new String(v);
}

Ref<System::String> Double::ToString()
{
	char v[128];

	sprintf_s(v, 128, "%lf", _v);

	return new String(v);
}
Ref<System::String> Char::ToString()
{
	wchar_t v[128];

	swprintf_s(v, 128, _T("%c"), _v);

	return new String(v);
}

System::Byte Byte::Parse(Ref<System::String>  value)
{
	Byte v;
	wscanf_s(value->c_str(), _T("%d"), &v._v);
	return v;
}

System::Char Char::Parse(Ref<System::String>  value)
{
	Char v;
	short temp;
	wscanf_s(value->c_str(), _T("%hd"), &temp);
	v._v = temp;
	return v;
}

System::Double Double::Parse(Ref<System::String>  value)
{
	Double v;
	wscanf_s(value->c_str(), _T("%lf"), &v._v);
	return v;
}
System::Int16 Int16::Parse(Ref<System::String>  value)
{
	Int16 v;
	wscanf_s(value->c_str(), _T("%hd"), &v._v);
	return v;
}
System::Int32 Int32::Parse(Ref<System::String>  value)
{
	Int32 v;
	wscanf_s(value->c_str(), _T("%d"), &v._v);
	return v;
}
System::Int64 Int64::Parse(Ref<System::String>  value)
{
	Int64 v;
	wscanf_s(value->c_str(), _T("%lld"), &v._v);
	return v;
}
System::SByte SByte::Parse(Ref<System::String>  value)
{
	SByte v;
	wscanf_s(value->c_str(), _T("%hhd"), &v._v);
	return v;
}
System::Single Single::Parse(Ref<System::String>  value)
{
	Single v;
	wscanf_s(value->c_str(), _T("%f"), &v._v);
	return v;
}
System::UInt16 UInt16::Parse(Ref<System::String>  value)
{
	UInt16 v;
	wscanf_s(value->c_str(), _T("%hu"), &v._v);
	return v;
}
System::UInt32 UInt32::Parse(Ref<System::String>  value)
{
	UInt32 v;
	wscanf_s(value->c_str(), _T("%u"), &v._v);
	return v;
}
System::UInt64 UInt64::Parse(Ref<System::String>  value)
{
	UInt64 v;
	wscanf_s(value->c_str(), _T("%llu"), &v._v);
	return v;
}
