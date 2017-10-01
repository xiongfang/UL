public:
	String(const char* c) 
	{ 
		int size = strlen(c) + 1;
		_v = new char[size];
		sprintf_s(_v, size, "%s", c);
	}
	~String() { delete[] _v; }
private:
	char* _v;
public:
	const char* c_str() { return _v; }