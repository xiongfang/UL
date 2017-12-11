public:
	std::vector<Ref<T>> _objs;

	System::Int32 get_Length()
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

	ArrayT(System::Int32 len)
	{
		_objs.resize(len._v);
	}