public:
	int _refCount;
	Object() 
	{ 
		_refCount = 0; 
	}
	virtual ~Object() {
	}
	void AddRef() { _refCount++; }
	void Release() 
	{ 
		_refCount--;
		if (_refCount == 0)
		{
			delete this;
		}
	}