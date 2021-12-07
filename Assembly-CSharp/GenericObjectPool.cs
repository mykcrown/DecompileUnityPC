using System;
using System.Collections.Generic;

// Token: 0x02000B4D RID: 2893
public class GenericObjectPool<T> where T : class
{
	// Token: 0x060053EA RID: 21482 RVA: 0x001B08F3 File Offset: 0x001AECF3
	public GenericObjectPool(int initialBufferSize, GenericObjectPool<T>.NewCallback newCallback, Action<T> ResetAction, Action<T> OnetimeInitAction = null)
	{
		this.m_objectStack = new Stack<T>(initialBufferSize);
		this.m_onetimeInitAction = OnetimeInitAction;
		this.m_newCallback = newCallback;
		this.m_resetAction = ResetAction;
	}

	// Token: 0x060053EB RID: 21483 RVA: 0x001B0920 File Offset: 0x001AED20
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

	// Token: 0x060053EC RID: 21484 RVA: 0x001B097C File Offset: 0x001AED7C
	public void Store(T obj)
	{
		this.m_objectStack.Push(obj);
	}

	// Token: 0x04003545 RID: 13637
	private Stack<T> m_objectStack;

	// Token: 0x04003546 RID: 13638
	private Action<T> m_resetAction;

	// Token: 0x04003547 RID: 13639
	private Action<T> m_onetimeInitAction;

	// Token: 0x04003548 RID: 13640
	private GenericObjectPool<T>.NewCallback m_newCallback;

	// Token: 0x02000B4E RID: 2894
	// (Invoke) Token: 0x060053EE RID: 21486
	public delegate T NewCallback();
}
