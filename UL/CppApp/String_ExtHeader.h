public:
	String(const char* c) { _v = c; }
private:
	const char* _v;
public:
	const char* c_str() { return _v; }