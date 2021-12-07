using System;
using UnityEngine;

// Token: 0x0200048C RID: 1164
public class PreloadDef : IEquatable<PreloadDef>
{
	// Token: 0x06001940 RID: 6464 RVA: 0x000841F4 File Offset: 0x000825F4
	public PreloadDef(GameObject obj, PreloadType type = PreloadType.EFFECT)
	{
		this.obj = obj;
		this.type = type;
	}

	// Token: 0x17000530 RID: 1328
	// (get) Token: 0x06001941 RID: 6465 RVA: 0x0008420A File Offset: 0x0008260A
	// (set) Token: 0x06001942 RID: 6466 RVA: 0x00084212 File Offset: 0x00082612
	public PreloadType type { get; private set; }

	// Token: 0x17000531 RID: 1329
	// (get) Token: 0x06001943 RID: 6467 RVA: 0x0008421B File Offset: 0x0008261B
	// (set) Token: 0x06001944 RID: 6468 RVA: 0x00084223 File Offset: 0x00082623
	public GameObject obj { get; private set; }

	// Token: 0x06001945 RID: 6469 RVA: 0x0008422C File Offset: 0x0008262C
	public bool Equals(PreloadDef compare)
	{
		return compare != null && compare.type == this.type && compare.obj == this.obj;
	}

	// Token: 0x06001946 RID: 6470 RVA: 0x0008425C File Offset: 0x0008265C
	public override bool Equals(object compare)
	{
		return compare is PreloadDef && this.Equals(compare as PreloadDef);
	}

	// Token: 0x06001947 RID: 6471 RVA: 0x00084278 File Offset: 0x00082678
	public override int GetHashCode()
	{
		return HashCode.Of<PreloadType>(this.type).And<GameObject>(this.obj);
	}
}
