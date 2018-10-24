#pragma once

#include <vector>
#include <map>


class Handle
{
	long long value;
public:
	Handle()
	{
		value = 0;
	}

	Handle(long v)
	{
		value = v;
	}

	Handle(const Handle& v)
	{
		value = v.value;
	}

	unsigned int Magic()const
	{
		return value >> 32;
	}

	unsigned int ID()const
	{
		return value & 0xffffffff;
	}

	unsigned int Index()const
	{
		return ID() - 1;
	}

	void SetMagic(unsigned int m)
	{
		long long magic = m;
		magic <<= 32;
		value = magic + ID();
	}

	void SetID(unsigned int id)
	{
		long long magic = Magic();
		magic <<= 32;
		value = magic + id;
	}

	bool operator==(const Handle& other)
	{
		return value == other.value;
	}
};

template<typename T>
class HandleManager
{
	
	std::vector<Handle> Handles;
	std::vector<T*> Objects;
	std::vector<Handle> unusedHandles;
public:

	Handle Alloc(T* v)
	{
		Handle h;
		if (unusedHandles.size() > 0)
		{
			h = unusedHandles.back();
			unusedHandles.pop_back();
			h.SetMagic(h.Magic() + 1);
		}
		else
		{
			h.SetID(Objects.size()+1);
			h.SetMagic(1);
			Objects.push_back(nullptr);
			Handles.push_back(h);
		}

		Objects[h.Index()] = v;
		Handles[h.Index()] = h;
		return h;
	}

	T* Release(const Handle& h)
	{
		if (!IsValid(h))
			return nullptr;

		T* v = Objects[h.Index()];
		unusedHandles.push_back(h);
		Objects[h.Index()] = nullptr;
		Handles[h.Index()] = Handle();

		return v;
	}

	bool IsValid(const Handle& h)
	{
		if (h.ID() > 0 && h.ID() <= Objects.size())
		{
			if (Handles[h.Index()] == h)
			{
				return true;
			}
		}

		return false;
	}

	T* Get(const Handle& h)
	{
		if (!IsValid(h))
			return nullptr;
		return Objects[h.Index()];
	}
};


