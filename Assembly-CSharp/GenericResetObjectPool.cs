using System;
using System.Collections.Generic;

// Token: 0x02000B4B RID: 2891
public class GenericResetObjectPool<T> : IResetObjectPool<T>, ICountOwner where T : class, IResetable
{
	// Token: 0x060053E2 RID: 21474 RVA: 0x001B0857 File Offset: 0x001AEC57
	public GenericResetObjectPool(int initialBufferSize, GenericResetObjectPool<T>.NewCallback newCallback, Action<T> OnetimeInitAction = null)
	{
		this.m_objectStack = new Stack<T>(initialBufferSize);
		this.m_onetimeInitAction = OnetimeInitAction;
		this.m_newCallback = newCallback;
	}

	// Token: 0x1700136D RID: 4973
	// (get) Token: 0x060053E3 RID: 21475 RVA: 0x001B0879 File Offset: 0x001AEC79
	public int Count
	{
		get
		{
			return this.m_objectStack.Count;
		}
	}

	// Token: 0x060053E4 RID: 21476 RVA: 0x001B0888 File Offset: 0x001AEC88
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

	// Token: 0x060053E5 RID: 21477 RVA: 0x001B08E5 File Offset: 0x001AECE5
	public void Store(T obj)
	{
		this.m_objectStack.Push(obj);
	}

	// Token: 0x04003542 RID: 13634
	private Stack<T> m_objectStack;

	// Token: 0x04003543 RID: 13635
	private Action<T> m_onetimeInitAction;

	// Token: 0x04003544 RID: 13636
	private GenericResetObjectPool<T>.NewCallback m_newCallback;

	// Token: 0x02000B4C RID: 2892
	// (Invoke) Token: 0x060053E7 RID: 21479
	public delegate T NewCallback();
}
