private:
	wchar_t* _v;
	int _length;
	String(const String& other){}
public:
	String(const char* c);
	String(const wchar_t* c);

	~String() { 
		delete[] _v; 
		
	}

public:
	wchar_t* c_str() { return _v; }