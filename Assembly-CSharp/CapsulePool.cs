using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class CapsulePool : ICapsuleOwner
{
	// Token: 0x06001360 RID: 4960 RVA: 0x0006FC5D File Offset: 0x0006E05D
	public CapsulePool()
	{
		this.HitBoxMaterial = (Resources.Load("Game/Effects/Materials/HitboxMaterial") as Material);
	}

	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06001361 RID: 4961 RVA: 0x0006FC85 File Offset: 0x0006E085
	public static CapsulePool Instance
	{
		get
		{
			if (CapsulePool.instance == null)
			{
				CapsulePool.instance = new CapsulePool();
			}
			return CapsulePool.instance;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x06001362 RID: 4962 RVA: 0x0006FCA0 File Offset: 0x0006E0A0
	// (set) Token: 0x06001363 RID: 4963 RVA: 0x0006FCA8 File Offset: 0x0006E0A8
	public Material HitBoxMaterial { get; private set; }

	// Token: 0x06001364 RID: 4964 RVA: 0x0006FCB4 File Offset: 0x0006E0B4
	public CapsuleShape GetCapsule(Transform transform = null)
	{
		if (this.capsules.Count <= 0)
		{
			Material material = UnityEngine.Object.Instantiate<Material>(this.HitBoxMaterial);
			this.currentIndex = (this.currentIndex + 1) % 255;
			material.SetInt("_Index", this.currentIndex);
			CapsuleShape capsuleShape = new GameObject("Capsule_" + this.currentIndex).AddComponent<CapsuleShape>();
			capsuleShape.transform.SetParent(transform, false);
			capsuleShape.tag = Tags.Debug;
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.GetComponent<MeshRenderer>().material = material;
			gameObject.transform.SetParent(capsuleShape.transform, false);
			gameObject.tag = Tags.Debug;
			UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<Collider>());
			gameObject.name = "Sphere1";
			GameObject gameObject2 = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject2.GetComponent<MeshRenderer>().material = material;
			gameObject2.transform.SetParent(capsuleShape.transform, false);
			gameObject2.tag = Tags.Debug;
			UnityEngine.Object.DestroyImmediate(gameObject2.GetComponent<Collider>());
			gameObject2.name = "Sphere2";
			GameObject gameObject3 = new GameObject("CylinderContainer");
			gameObject3.transform.SetParent(capsuleShape.transform, false);
			gameObject3.tag = Tags.Debug;
			GameObject gameObject4 = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
			gameObject4.tag = Tags.Debug;
			gameObject4.GetComponent<MeshRenderer>().material = material;
			UnityEngine.Object.DestroyImmediate(gameObject4.GetComponent<Collider>());
			gameObject4.transform.SetParent(gameObject3.transform, false);
			Vector3 localPosition = new Vector3(0f, 0f, 1f);
			Quaternion localRotation = Quaternion.Euler(90f, 0f, 0f);
			gameObject4.transform.localPosition = localPosition;
			gameObject4.transform.localRotation = localRotation;
			capsuleShape.Initialize(gameObject, gameObject2, gameObject3, material);
			capsuleShape.SetOwner(this);
			return capsuleShape;
		}
		CapsuleShape capsuleShape2 = this.capsules.Pop();
		if (capsuleShape2 == null)
		{
			this.capsules.Clear();
			return this.GetCapsule(transform);
		}
		capsuleShape2.enabled = true;
		return capsuleShape2;
	}

	// Token: 0x06001365 RID: 4965 RVA: 0x0006FEC9 File Offset: 0x0006E2C9
	public void ReturnCapsule(CapsuleShape capsule)
	{
		if (capsule == null)
		{
			Debug.LogWarning("Attempted to return a null capsule");
			return;
		}
		capsule.Visible = false;
		capsule.enabled = false;
		this.capsules.Push(capsule);
	}

	// Token: 0x04000CCC RID: 3276
	private static CapsulePool instance;

	// Token: 0x04000CCD RID: 3277
	private Stack<CapsuleShape> capsules = new Stack<CapsuleShape>();

	// Token: 0x04000CCE RID: 3278
	private int currentIndex;
}
