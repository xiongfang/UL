int _v;
public:
	operator bool()
	{
		return _v!=0;
	}
	Boolean() { _v = 0; }
	Boolean(bool v)
	{
		_v = v?1:0;
	}