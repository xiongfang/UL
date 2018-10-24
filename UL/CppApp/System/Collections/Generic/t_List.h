#pragma once
#include "System\Object.h"
#include "System\Int32.h"
#include "System\t_ArrayT.h"
namespace System{
	namespace Collections{
		namespace Generic{
			template<class T>
			class List:public System::Object
			{
			#include "List_Ext.h"
			};
		}
	}
}
