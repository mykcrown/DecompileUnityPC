using System;

namespace InControl
{
	// Token: 0x020001D5 RID: 469
	internal class RingBuffer<T>
	{
		// Token: 0x06000821 RID: 2081 RVA: 0x0004AB48 File Offset: 0x00048F48
		public RingBuffer(int size)
		{
			if (size <= 0)
			{
				throw new ArgumentException("RingBuffer size must be 1 or greater.");
			}
			this.size = size + 1;
			this.data = new T[this.size];
			this.head = 0;
			this.tail = 0;
			this.sync = new object();
		}

		// Token: 0x06000822 RID: 2082 RVA: 0x0004ABA0 File Offset: 0x00048FA0
		public void Enqueue(T value)
		{
			object obj = this.sync;
			lock (obj)
			{
				if (this.size > 1)
				{
					this.head = (this.head + 1) % this.size;
					if (this.head == this.tail)
					{
						this.tail = (this.tail + 1) % this.size;
					}
				}
				this.data[this.head] = value;
			}
		}

		// Token: 0x06000823 RID: 2083 RVA: 0x0004AC38 File Offset: 0x00049038
		public T Dequeue()
		{
			object obj = this.sync;
			T result;
			lock (obj)
			{
				if (this.size > 1 && this.tail != this.head)
				{
					this.tail = (this.tail + 1) % this.size;
				}
				result = this.data[this.tail];
			}
			return result;
		}

		// Token: 0x040005DD RID: 1501
		private int size;

		// Token: 0x040005DE RID: 1502
		private T[] data;

		// Token: 0x040005DF RID: 1503
		private int head;

		// Token: 0x040005E0 RID: 1504
		private int tail;

		// Token: 0x040005E1 RID: 1505
		private object sync;
	}
}
