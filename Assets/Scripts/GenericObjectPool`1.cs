// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

public class GenericObjectPool<T> where T : class
{
	public delegate T NewCallback();

	private Stack<T> m_objectStack;

	private Action<T> m_resetAction;

	private Action<T> m_onetimeInitAction;

	private GenericObjectPool<T>.NewCallback m_newCallback;

	public GenericObjectPool(int initialBufferSize, GenericObjectPool<T>.NewCallback newCallback, Action<T> ResetAction, Action<T> OnetimeInitAction = null)
	{
		this.m_objectStack = new Stack<T>(initialBufferSize);
		this.m_onetimeInitAction = OnetimeInitAction;
		this.m_newCallback = newCallback;
		this.m_resetAction = ResetAction;
	}

	public T New()
	{
		if (this.m_objectStack.Count > 0)
		{
			T t = this.m_objectStack.Pop();
			this.m_resetAction(t);
			return t;
		}
		T t2 = this.m_newCallback();
		if (this.m_onetimeInitAction != null)
		{
			this.m_onetimeInitAction(t2);
		}
		return t2;
	}

	public void Store(T obj)
	{
		this.m_objectStack.Push(obj);
	}
}
