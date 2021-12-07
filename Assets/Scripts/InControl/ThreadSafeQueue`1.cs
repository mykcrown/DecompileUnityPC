// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;

namespace InControl
{
	internal class ThreadSafeQueue<T>
	{
		private object sync;

		private Queue<T> data;

		public ThreadSafeQueue()
		{
			this.sync = new object();
			this.data = new Queue<T>();
		}

		public ThreadSafeQueue(int capacity)
		{
			this.sync = new object();
			this.data = new Queue<T>(capacity);
		}

		public void Enqueue(T item)
		{
			object obj = this.sync;
			lock (obj)
			{
				this.data.Enqueue(item);
			}
		}

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
	}
}
