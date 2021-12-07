using System;
using System.Collections;

// Token: 0x02000AAB RID: 2731
public interface ICustomList
{
	// Token: 0x0600503F RID: 20543
	void Clear();

	// Token: 0x170012EA RID: 4842
	// (get) Token: 0x06005040 RID: 20544
	int Count { get; }

	// Token: 0x170012EB RID: 4843
	// (get) Token: 0x06005041 RID: 20545
	bool IsReadOnly { get; }

	// Token: 0x06005042 RID: 20546
	void RemoveAt(int index);

	// Token: 0x06005043 RID: 20547
	IEnumerator ManualGetEnumerator();
}
