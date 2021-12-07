using System;
using System.Collections.Generic;

namespace InControl
{
	// Token: 0x020001D8 RID: 472
	internal class ThreadSafeQueue<T>
	{
		// Token: 0x0600082D RID: 2093 RVA: 0x0004ACCB File Offset: 0x000490CB
		public ThreadSafeQueue()
		{
			this.sync = new object();
			this.data = new Queue<T>();
		}

		// Token: 0x0600082E RID: 2094 RVA: 0x0004ACE9 File Offset: 0x000490E9
		public ThreadSafeQueue(int capacity)
		{
			this.sync = new object();
			this.data = new Queue<T>(capacity);
		}

		// Token: 0x0600082F RID: 2095 RVA: 0x0004AD08 File Offset: 0x00049108
		public void Enqueue(T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				this.data.Enqueue(item);
			}
		}

		// Token: 0x06000830 RID: 2096 RVA: 0x0004AD54 File Offset: 0x00049154
		public bool Dequeue(out T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.data.Count > 0)
				{
					item = this.data.Dequeue();
					return true;
				}
			}
			item = default(T);
			return false;
		}

		// Token: 0x06000831 RID: 2097 RVA: 0x0004ADCC File Offset: 0x000491CC
		public T Dequeue()
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.data.Count > 0)
				{
					return this.data.Dequeue();
				}
			}
			return default(T);
		}

		// Token: 0x06000832 RID: 2098 RVA: 0x0004AE38 File Offset: 0x00049238
		public int Dequeue(ref IList<T> list)
		{
			object obj = this.sync;
			int result;
			lock (obj)
			{
				int count = this.data.Count;
				for (int i = 0; i < count; i++)
				{
					list.Add(this.data.Dequeue());
				}
				result = count;
			}
			return result;
		}

		// Token: 0x040005E6 RID: 1510
		private object sync;

		// Token: 0x040005E7 RID: 1511
		private Queue<T> data;
	}
}
