using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000468 RID: 1128
public interface ICoroutineStarter
{
	// Token: 0x060017DA RID: 6106
	Coroutine StartCoroutine(IEnumerator routine);
}
