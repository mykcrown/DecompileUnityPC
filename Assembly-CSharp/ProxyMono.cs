using System;
using UnityEngine;

// Token: 0x02000B32 RID: 2866
public class ProxyMono : MonoBehaviour
{
	// Token: 0x06005326 RID: 21286 RVA: 0x001AE32D File Offset: 0x001AC72D
	protected void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(this);
	}

	// Token: 0x06005327 RID: 21287 RVA: 0x001AE335 File Offset: 0x001AC735
	private void Update()
	{
		if (this.OnUpdate != null)
		{
			this.OnUpdate();
		}
	}

	// Token: 0x06005328 RID: 21288 RVA: 0x001AE34D File Offset: 0x001AC74D
	private void LateUpdate()
	{
		if (this.OnLateUpdate != null)
		{
			this.OnLateUpdate();
		}
	}

	// Token: 0x06005329 RID: 21289 RVA: 0x001AE365 File Offset: 0x001AC765
	public void OnDestroy()
	{
		if (this.OnDestroyFn != null)
		{
			this.OnDestroyFn();
		}
	}

	// Token: 0x0600532A RID: 21290 RVA: 0x001AE37D File Offset: 0x001AC77D
	public void OnApplicationFocus(bool hasFocus)
	{
		if (this.OnApplicationFocusFn != null)
		{
			this.OnApplicationFocusFn(hasFocus);
		}
	}

	// Token: 0x0600532B RID: 21291 RVA: 0x001AE398 File Offset: 0x001AC798
	public static ProxyMono CreateNew(string name)
	{
		ProxyMono proxyMono = new GameObject(name).AddComponent<ProxyMono>();
		ProxyMono.Attach(proxyMono.gameObject);
		return proxyMono;
	}

	// Token: 0x0600532C RID: 21292 RVA: 0x001AE3BD File Offset: 0x001AC7BD
	public static void Attach(GameObject obj)
	{
		obj.transform.SetParent(GameObject.Find("ProxyMonoObjects").transform);
	}

	// Token: 0x040034D4 RID: 13524
	public Action OnUpdate;

	// Token: 0x040034D5 RID: 13525
	public Action OnLateUpdate;

	// Token: 0x040034D6 RID: 13526
	public Action OnDestroyFn;

	// Token: 0x040034D7 RID: 13527
	public Action<bool> OnApplicationFocusFn;
}
