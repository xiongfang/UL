#pragma once

#include "Common.h"


enum ECode:uint8
{
	nop,	//什么也不做，使程序计数器+1，用来对齐字节码
	ldc, //将一个数值常量值压栈
	stloc,		//将栈上的值弹回本地变量
	starg,		//将栈上的值存储到参数
	ret,		//返回指令，依据当前调用的函数是否有返回值，此指令将当前栈上的返回值，放置到调用者栈上
	call,		//调用指令
	ldarg,		//加载一个参数到栈
	ldarga,		//加载参数地址，（用来引用传递参数）
	ldloc,		//加载本地变量到栈
	ldloca,		//加载本地变量的地址到栈上
	stind,		//存储值到间接地址
	initobj,	//初始化地址处的值
	newobj,		//新建一个对象并调用构造函数
	box,		//将栈上的值类型装箱为指定的类型，如果是引用类型，则不变
	unbox,		//将值类型对象的值地址存储到栈
	unboxany,	//将值类型拆箱
	ldobj,		//将地址指向的值加载到栈
};

namespace Command
{
	struct NOP
	{
		ECode Code;
		RET()
		{
			Code = ECode::nop;
		}
	};

	

	struct LDC
	{
		enum EValueType
		{
			Int32,
			Int64,
			Float32,
			Float64
		};
		ECode Code;
		EValueType Type;
		//要放入栈的值
		union UValue
		{
			uint64 IValue;
			float64 FValue;
		}Value;
		
		LDC()
		{
			Code = ECode::ldc;
			Type = Int32;
			Value.IValue = 0;
		}
	};

	struct STLOC
	{
		ECode Code;
		uint16 Index;
		STLOC()
		{
			Code = ECode::stloc;
			Index = 0;
		}
	};
	struct STARG
	{
		ECode Code;
		uint16 Index;
		STLOC()
		{
			Code = ECode::starg;
			Index = 0;
		}
	};

	struct RET
	{
		ECode Code;
		RET()
		{
			Code = ECode::ret;
		}
	};

	struct CALL
	{
		ECode Code;
		int32 MethodHandle;
		CALL()
		{
			Code = ECode::call;
			Method = 0;
		}
	};

	struct LDARG
	{
		ECode Code;
		uint8 Index;
		LDARG()
		{
			Code = ECode::ldarg;
			Index = 0;
		}
	};

	struct LDARGA
	{
		ECode Code;
		uint8 Index;
		LDARG()
		{
			Code = ECode::ldarga;
			Index = 0;
		}
	};

	struct  LDLOC
	{
		ECode Code;
		uint8 Index;
		LDLOC()
		{
			Code = ECode::ldloc;
			Index = 0;
		}
	};

	struct LDLOCA
	{
		ECode Code;
		uint8 Index;
		LDLOCA()
		{
			Code = ECode::ldloca;
			Index = 0;
		}
	};

	struct STIND
	{
		enum EValueType
		{
			Int8,
			Int16,
			Int32,
			Int64,
			Float32,
			Float64,
			Ref,
		};

		ECode Code;
		EValueType Type;
		STIND()
		{
			Code = ECode::stind;
			Type = Int8;
		}
	};

	struct INITOBJ
	{
		ECode Code;
		int32 TypeHandle;
		INITOBJ()
		{
			Code = ECode::initobj;
			TypeHandle = 0;
		}
	};

	struct NEWOBJ
	{
		ECode Code;
		int32 TypeHandle;
		NEWOBJ()
		{
			Code = ECode::newobj;
			TypeHandle = 0;
		}
	};

	struct BOX
	{
		ECode Code;
		int32 TypeHandle;
		BOX()
		{
			Code = ECode::box;
			TypeHandle = 0;
		}
	};
}
//
//class Command
//{
//	public ECode Code;
//};
//
//class Command_Invoke :public Command
//{
//public:
//	//注册函数地址
//	int FuncID;
//	Command_Invoke() { Code = ECode.Invoke; }
//};
//
//class Command_Call :public Command
//{
//public:
//	//代码地址
//	int CodeID;
//	Command_Call() { Code = ECode.Call; }
//};
//
//class Command_Ret :public Command
//{
//public:
//	Command_Ret() { Code = ECode.Ret; }
//};
//
//class Command_Push :public Command
//{
//public:
//	public Metadata.Const Value;
//	public Command_Push() { Code = ECode.Push; }
//};
//
//class Command_Pop :public Command
//{
//public:
//	//public int MemoryID; //内存地址(为0则pop一个对象，不存放到内存)
//	int Count;   //Pop数量
//	Command_Pop() { Code = ECode.Pop; }
//};
//
//class Command_Move :public Command
//{
//	
//public:
//	enum AddressType
//	{
//		EBP,
//		ESP
//	};
//
//	AddressType SourceType;
//	int SourceIndex;  //相对于EBP
//	AddressType DestType;
//	int DestIndex; //相对于EBP
//	Command_Move() { Code = ECode.Move; }
//};
//
//class Command_Jump :public Command
//{
//	//代码地址
//public:
//	int CodeID;
//	Command_Jump() { Code = ECode.Jump; }
//};
//
//class Command_JZ :public Command
//{
//	//代码地址
//public:
//	int CodeID;
//	Command_JZ() { Code = ECode.JZ; }
//};

class Runtime
{

};