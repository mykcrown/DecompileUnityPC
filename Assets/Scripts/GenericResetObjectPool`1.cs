// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GenericResetObjectPool<T> : IResetObjectPool<T>, ICountOwner where T : class, IResetable
{
	public delegate T NewCallback();

	private Stack<T> m_objectStack;

	private Action<T> m_onetimeInitAction;

	private GenericResetObjectPool<T>.NewCallback m_newCallback;

	public int Count
	{
		get
		{
			return this.m_objectStack.Count;
		}
	}

	public GenericResetObjectPool(int initialBufferSize, GenericResetObjectPool<T>.NewCallback newCallback, Action<T> OnetimeInitAction = null)
	{
		this.m_objectStack = new Stack<T>(initialBufferSize);
		this.m_onetimeInitAction = OnetimeInitAction;
		this.m_newCallback = newCallback;
	}

	public T New()
	{
		if (this.m_objectStack.Count > 0)
		{
			T result = this.m_objectStack.Pop();
			result.Reset();
			return result;
		}
		T t = this.m_newCallback();
		if (this.m_onetimeInitAction != null)
		{
			this.m_onetimeInitAction(t);
		}
		return t;
	}

	public void Store(T obj)
	{
		this.m_objectStack.Push(obj);
	}
}
