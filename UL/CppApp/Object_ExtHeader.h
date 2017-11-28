private:
	Handle id;
	int _refCount;
	

public:
	static HandleManager<Object> Mgr;
	Object() 
	{ 
		_refCount = 0; 
	}
	virtual ~Object() {
	}
	void AddRef() 
	{ 
		if (_refCount == 0)
		{
			id = Mgr.Alloc(this);
		}
		_refCount++; 
	}
	void Release() 
	{ 
		_refCount--;
		if (_refCount == 0)
		{
			Mgr.Release(id);
			delete this;
		}
	}

	Handle __GetID() { return id; }

	//template<typename T>
	//static T* New()
	//{
	//	Object* p = new T();
	//	p->id = Mgr.Alloc(p);
	//	return p;
	//}

	//template<typename T>
	//static void Delete(T* p)
	//{
	//	Object* obj = (Object*)p;
	//	Mgr.Release(obj->id);
	//	free(p);
	//}