// Decompile from assembly: Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

public class CapsulePool : ICapsuleOwner
{
	private static CapsulePool instance;

	private Stack<CapsuleShape> capsules = new Stack<CapsuleShape>();

	private int currentIndex;

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

	public Material HitBoxMaterial
	{
		get;
		private set;
	}

	public CapsulePool()
	{
		this.HitBoxMaterial = (Resources.Load("Game/Effects/Materials/HitboxMaterial") as Material);
	}

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

	public void ReturnCapsule(CapsuleShape capsule)
	{
		if (capsule == null)
		{
			UnityEngine.Debug.LogWarning("Attempted to return a null capsule");
			return;
		}
		capsule.Visible = false;
		capsule.enabled = false;
		this.capsules.Push(capsule);
	}
}
