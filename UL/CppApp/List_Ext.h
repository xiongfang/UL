public:
	std::vector<Ref<T>> _objs;

	System::Int32 get_Count()
	{
		return _objs.size();
	}

	Ref<T> get_Index(System::Int32  i)
	{
		return _objs[i._v];
	}

	void set_Index(System::Int32  i, Ref<T> value)
	{
		_objs[i._v] = value;
	}

	void Add(Ref<T> value)
	{
		_objs.push_back(value);
	}
	void Remove(Ref<T> value)
	{
		
	}
	void RemoveAll(Ref<T> value)
	{

	}

	Ref<ArrayT<T>> ToArray()
	{
		ArrayT<T>* v = new ArrayT<T>(get_Count());
		v->_objs = _objs;
		return v;
	}