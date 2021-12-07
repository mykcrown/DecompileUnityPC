using System;
using UnityEngine;

// Token: 0x02000010 RID: 16
public class OrbitCamera : MonoBehaviour
{
	// Token: 0x0600005B RID: 91 RVA: 0x00005B24 File Offset: 0x00003F24
	private void Start()
	{
		this.Distance = Vector3.Distance(this.TargetLookAt.transform.position, base.gameObject.transform.position);
		if (this.Distance > this.DistanceMax)
		{
			this.DistanceMax = this.Distance;
		}
		this.startingDistance = this.Distance;
		this.Reset();
	}

	// Token: 0x0600005C RID: 92 RVA: 0x00005B8B File Offset: 0x00003F8B
	private void Update()
	{
	}

	// Token: 0x0600005D RID: 93 RVA: 0x00005B8D File Offset: 0x00003F8D
	private void LateUpdate()
	{
		if (this.TargetLookAt == null)
		{
			return;
		}
		this.HandlePlayerInput();
		this.CalculateDesiredPosition();
		this.UpdatePosition();
	}

	// Token: 0x0600005E RID: 94 RVA: 0x00005BB4 File Offset: 0x00003FB4
	private void HandlePlayerInput()
	{
		float num = 0.01f;
		if (Input.GetMouseButton(0))
		{
			this.mouseX += Input.GetAxis("Mouse X") * this.X_MouseSensitivity;
			this.mouseY -= Input.GetAxis("Mouse Y") * this.Y_MouseSensitivity;
		}
		this.mouseY = this.ClampAngle(this.mouseY, this.Y_MinLimit, this.Y_MaxLimit);
		if (Input.GetAxis("Mouse ScrollWheel") < -num || Input.GetAxis("Mouse ScrollWheel") > num)
		{
			this.desiredDistance = Mathf.Clamp(this.Distance - Input.GetAxis("Mouse ScrollWheel") * this.MouseWheelSensitivity, this.DistanceMin, this.DistanceMax);
		}
	}

	// Token: 0x0600005F RID: 95 RVA: 0x00005C7C File Offset: 0x0000407C
	private void CalculateDesiredPosition()
	{
		this.Distance = Mathf.SmoothDamp(this.Distance, this.desiredDistance, ref this.velocityDistance, this.DistanceSmooth);
		this.desiredPosition = this.CalculatePosition(this.mouseY, this.mouseX, this.Distance);
	}

	// Token: 0x06000060 RID: 96 RVA: 0x00005CCC File Offset: 0x000040CC
	private Vector3 CalculatePosition(float rotationX, float rotationY, float distance)
	{
		Vector3 point = new Vector3(0f, 0f, -distance);
		Quaternion rotation = Quaternion.Euler(rotationX, rotationY, 0f);
		return this.TargetLookAt.position + rotation * point;
	}

	// Token: 0x06000061 RID: 97 RVA: 0x00005D10 File Offset: 0x00004110
	private void UpdatePosition()
	{
		float x = Mathf.SmoothDamp(this.position.x, this.desiredPosition.x, ref this.velX, this.X_Smooth);
		float y = Mathf.SmoothDamp(this.position.y, this.desiredPosition.y, ref this.velY, this.Y_Smooth);
		float z = Mathf.SmoothDamp(this.position.z, this.desiredPosition.z, ref this.velZ, this.X_Smooth);
		this.position = new Vector3(x, y, z);
		base.transform.position = this.position;
		base.transform.LookAt(this.TargetLookAt);
	}

	// Token: 0x06000062 RID: 98 RVA: 0x00005DC5 File Offset: 0x000041C5
	private void Reset()
	{
		this.mouseX = 0f;
		this.mouseY = 0f;
		this.Distance = this.startingDistance;
		this.desiredDistance = this.Distance;
	}

	// Token: 0x06000063 RID: 99 RVA: 0x00005DF8 File Offset: 0x000041F8
	private float ClampAngle(float angle, float min, float max)
	{
		while (angle < -360f || angle > 360f)
		{
			if (angle < -360f)
			{
				angle += 360f;
			}
			if (angle > 360f)
			{
				angle -= 360f;
			}
		}
		return Mathf.Clamp(angle, min, max);
	}

	// Token: 0x040000A0 RID: 160
	public Transform TargetLookAt;

	// Token: 0x040000A1 RID: 161
	public float Distance = 10f;

	// Token: 0x040000A2 RID: 162
	public float DistanceMin = 5f;

	// Token: 0x040000A3 RID: 163
	public float DistanceMax = 15f;

	// Token: 0x040000A4 RID: 164
	private float startingDistance;

	// Token: 0x040000A5 RID: 165
	private float desiredDistance;

	// Token: 0x040000A6 RID: 166
	private float mouseX;

	// Token: 0x040000A7 RID: 167
	private float mouseY;

	// Token: 0x040000A8 RID: 168
	public float X_MouseSensitivity = 5f;

	// Token: 0x040000A9 RID: 169
	public float Y_MouseSensitivity = 5f;

	// Token: 0x040000AA RID: 170
	public float MouseWheelSensitivity = 5f;

	// Token: 0x040000AB RID: 171
	public float Y_MinLimit = 15f;

	// Token: 0x040000AC RID: 172
	public float Y_MaxLimit = 70f;

	// Token: 0x040000AD RID: 173
	public float DistanceSmooth = 0.025f;

	// Token: 0x040000AE RID: 174
	private float velocityDistance;

	// Token: 0x040000AF RID: 175
	private Vector3 desiredPosition = Vector3.zero;

	// Token: 0x040000B0 RID: 176
	public float X_Smooth = 0.05f;

	// Token: 0x040000B1 RID: 177
	public float Y_Smooth = 0.1f;

	// Token: 0x040000B2 RID: 178
	private float velX;

	// Token: 0x040000B3 RID: 179
	private float velY;

	// Token: 0x040000B4 RID: 180
	private float velZ;

	// Token: 0x040000B5 RID: 181
	private Vector3 position = Vector3.zero;
}
