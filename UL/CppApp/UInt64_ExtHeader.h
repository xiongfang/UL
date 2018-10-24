typedef unsigned long long ValueType;
private:
	ValueType _v;
public:
	UInt64() { _v = 0; }
	UInt64(ValueType v)
	{
		_v = v;
	}

