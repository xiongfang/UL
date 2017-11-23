#pragma once
#include "Object.h"

namespace System
{
	template<typename T,typename TrueType>
	class Number:public System::Object
	{
	protected:
		T _v;
	public:
		TrueType operator+(TrueType  b)
		{
			return TrueType(b._v + _v);
		}
		TrueType operator-(TrueType  b)
		{
			return TrueType(_v - b._v);
		}
		TrueType operator*(TrueType  b)
		{
			return TrueType(_v * b._v);
		}
		TrueType operator/(TrueType  b)
		{
			return TrueType(_v / b._v);
		}
		TrueType operator%(TrueType  b)
		{
			return TrueType(_v % b._v);
		}
		TrueType operator&(TrueType  b)
		{
			return TrueType(_v & b._v);
		}
		TrueType operator|(TrueType  b)
		{
			return TrueType(_v | b._v);
		}
		TrueType operator~()
		{
			return TrueType(~_v);
		}
		TrueType operator<<(TrueType  b)
		{
			return TrueType(_v << b._v);
		}
		TrueType operator>>(TrueType  b)
		{
			return TrueType(_v >> b._v);
		}

		TrueType& operator+=(TrueType  b)
		{
			_v += b._v;
			return (*(TrueType*)this);
		}
		TrueType& operator-=(TrueType  b)
		{
			_v -= b._v;
			return (*(TrueType*)this);
		}
		TrueType& operator*=(TrueType  b)
		{
			_v *= b._v;
			return (*(TrueType*)this);
		}
		TrueType& operator/=(TrueType  b)
		{
			_v /= b._v;
			return (*(TrueType*)this);
		}

		TrueType& operator%=(TrueType  b)
		{
			_v %= b._v;
			return (*(TrueType*)this);
		}
		TrueType operator&=(TrueType  b)
		{
			_v &= b._v;
			return (*(TrueType*)this);
		}
		TrueType operator|=(TrueType  b)
		{
			_v |= b._v;
			return (*(TrueType*)this);
		}
		TrueType operator<<=(TrueType  b)
		{
			_v <<= b._v;
			return (*(TrueType*)this);
		}
		TrueType operator>>=(TrueType  b)
		{
			_v >>= b._v;
			return (*(TrueType*)this);
		}

		TrueType& operator++()
		{
			_v += 1;
			return (*(TrueType*)this);
		}
		TrueType operator++(int)
		{
			TrueType temp(_v);
			_v += 1;
			return temp;
		}


	};
}
