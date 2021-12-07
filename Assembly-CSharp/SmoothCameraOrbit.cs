using System;
using UnityEngine;

// Token: 0x02000005 RID: 5
[AddComponentMenu("Camera-Control/Smooth Mouse Orbit - Unluck Software")]
public class SmoothCameraOrbit : MonoBehaviour
{
	// Token: 0x0600000E RID: 14 RVA: 0x00002476 File Offset: 0x00000876
	private void Start()
	{
		this.Init();
	}

	// Token: 0x0600000F RID: 15 RVA: 0x0000247E File Offset: 0x0000087E
	private void OnEnable()
	{
		this.Init();
	}

	// Token: 0x06000010 RID: 16 RVA: 0x00002488 File Offset: 0x00000888
	public void Init()
	{
		if (!this.target)
		{
			this.target = new GameObject("Cam Target")
			{
				transform = 
				{
					position = base.transform.position + base.transform.forward * this.distance
				}
			}.transform;
		}
		this.currentDistance = this.distance;
		this.desiredDistance = this.distance;
		this.position = base.transform.position;
		this.rotation = base.transform.rotation;
		this.currentRotation = base.transform.rotation;
		this.desiredRotation = base.transform.rotation;
		this.xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		this.yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
	}

	// Token: 0x06000011 RID: 17 RVA: 0x000025BC File Offset: 0x000009BC
	private void LateUpdate()
	{
		if (Input.GetMouseButton(2) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
		{
			this.desiredDistance -= Input.GetAxis("Mouse Y") * 0.02f * (float)this.zoomRate * 0.125f * Mathf.Abs(this.desiredDistance);
		}
		else if (Input.GetMouseButton(0))
		{
			this.xDeg += Input.GetAxis("Mouse X") * this.xSpeed * 0.02f;
			this.yDeg -= Input.GetAxis("Mouse Y") * this.ySpeed * 0.02f;
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening);
			base.transform.rotation = this.rotation;
			this.idleTimer = 0f;
			this.idleSmooth = 0f;
		}
		else
		{
			this.idleTimer += 0.02f;
			if (this.idleTimer > this.autoRotate && this.autoRotate > 0f)
			{
				this.idleSmooth += (0.02f + this.idleSmooth) * 0.005f;
				this.idleSmooth = Mathf.Clamp(this.idleSmooth, 0f, 1f);
				this.xDeg += this.xSpeed * Time.deltaTime * this.idleSmooth * this.autoRotateSpeed;
			}
			this.yDeg = SmoothCameraOrbit.ClampAngle(this.yDeg, (float)this.yMinLimit, (float)this.yMaxLimit);
			this.desiredRotation = Quaternion.Euler(this.yDeg, this.xDeg, 0f);
			this.currentRotation = base.transform.rotation;
			this.rotation = Quaternion.Lerp(this.currentRotation, this.desiredRotation, 0.02f * this.zoomDampening * 2f);
			base.transform.rotation = this.rotation;
		}
		this.desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * (float)this.zoomRate * Mathf.Abs(this.desiredDistance);
		this.desiredDistance = Mathf.Clamp(this.desiredDistance, this.minDistance, this.maxDistance);
		this.currentDistance = Mathf.Lerp(this.currentDistance, this.desiredDistance, 0.02f * this.zoomDampening);
		this.position = this.target.position - (this.rotation * Vector3.forward * this.currentDistance + this.targetOffset);
		base.transform.position = this.position;
	}

	// Token: 0x06000012 RID: 18 RVA: 0x000028F4 File Offset: 0x00000CF4
	private static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x0400000B RID: 11
	public Transform target;

	// Token: 0x0400000C RID: 12
	public Vector3 targetOffset;

	// Token: 0x0400000D RID: 13
	public float distance = 5f;

	// Token: 0x0400000E RID: 14
	public float maxDistance = 20f;

	// Token: 0x0400000F RID: 15
	public float minDistance = 0.6f;

	// Token: 0x04000010 RID: 16
	public float xSpeed = 200f;

	// Token: 0x04000011 RID: 17
	public float ySpeed = 200f;

	// Token: 0x04000012 RID: 18
	public int yMinLimit = -80;

	// Token: 0x04000013 RID: 19
	public int yMaxLimit = 80;

	// Token: 0x04000014 RID: 20
	public int zoomRate = 40;

	// Token: 0x04000015 RID: 21
	public float panSpeed = 0.3f;

	// Token: 0x04000016 RID: 22
	public float zoomDampening = 5f;

	// Token: 0x04000017 RID: 23
	public float autoRotate = 1f;

	// Token: 0x04000018 RID: 24
	public float autoRotateSpeed = 0.1f;

	// Token: 0x04000019 RID: 25
	private float xDeg;

	// Token: 0x0400001A RID: 26
	private float yDeg;

	// Token: 0x0400001B RID: 27
	private float currentDistance;

	// Token: 0x0400001C RID: 28
	private float desiredDistance;

	// Token: 0x0400001D RID: 29
	private Quaternion currentRotation;

	// Token: 0x0400001E RID: 30
	private Quaternion desiredRotation;

	// Token: 0x0400001F RID: 31
	private Quaternion rotation;

	// Token: 0x04000020 RID: 32
	private Vector3 position;

	// Token: 0x04000021 RID: 33
	private float idleTimer;

	// Token: 0x04000022 RID: 34
	private float idleSmooth;
}
