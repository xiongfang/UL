﻿#pragma once
#include "Object.h"
#include "String.h"
namespace System{
	struct Boolean:public System::Object
	{
		public:
		static 		Ref<System::String> FalseString;
		public:
		static 		Ref<System::String> TrueString;
		public:
		static System::Boolean Parse(Ref<System::String>  value);
		public:
		Ref<System::String> ToString();
		public:
		static System::Boolean TryParse(Ref<System::String>  value,System::Boolean & v);
		public:
		static System::Boolean op_LogicNot(System::Boolean  a);
	#include "Boolean_ExtHeader.h"
	};
}
