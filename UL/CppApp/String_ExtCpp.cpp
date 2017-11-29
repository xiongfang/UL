#include "stdafx.h"
#include "String.h"
#include "Object.h"
#include "Int32.h"
#include "Char.h"
#include "Array.h"


using namespace System;
System::Int32 String::get_Length()
{
	return _length;
}

System::Int32 String::IndexOf(System::Char  value)
{
	for (int i = 0; i < _length; i++)
	{
		if (_v[i] == value._v)
		{
			return i;
		}
	}

	return -1;
}

System::Int32 String::IndexOf(Ref<System::String>  value)
{
	for (int i = 0; i < _length; i++)
	{
		bool match = true;
		for (int j = 0; j < value->get_Length()._v; j++)
		{
			if ( i+j>=_length || _v[i + j] != value->_v[j])
			{
				match = false;
				break;
			}
		}

		if(match)
			return i;
	}

	return -1;
}

System::Int32 String::LastIndexOf(System::Char  value)
{
	for (int i = _length-1; i >=0; i--)
	{
		if (_v[i] == value._v)
		{
			return i;
		}
	}

	return -1;
}

System::Int32 String::LastIndexOf(Ref<System::String>  value)
{
	for (int i = _length-value->_length; i >=0; i--)
	{
		bool match = true;
		for (int j = 0; j < value->get_Length()._v; j++)
		{
			if (i + j >= _length || _v[i + j] != value->_v[j])
			{
				match = false;
				break;
			}
		}

		if (match)
			return i;
	}

	return -1;
}