using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000006 RID: 6
public class FlockChild : MonoBehaviour
{
	// Token: 0x06000014 RID: 20 RVA: 0x00002954 File Offset: 0x00000D54
	public void Start()
	{
		this.FindRequiredComponents();
		this.Wander(0f);
		this.SetRandomScale();
		this._thisT.position = this.findWaypoint();
		this.RandomizeStartAnimationFrame();
		this.InitAvoidanceValues();
		this._speed = this._spawner._minSpeed;
		this._spawner._activeChildren += 1f;
		this._instantiated = true;
		if (this._spawner._updateDivisor > 1)
		{
			int num = this._spawner._updateDivisor - 1;
			FlockChild._updateNextSeed++;
			this._updateSeed = FlockChild._updateNextSeed;
			FlockChild._updateNextSeed %= num;
		}
	}

	// Token: 0x06000015 RID: 21 RVA: 0x00002A08 File Offset: 0x00000E08
	public void Update()
	{
		if (this._spawner._updateDivisor <= 1 || this._spawner._updateCounter == this._updateSeed)
		{
			this.SoarTimeLimit();
			this.CheckForDistanceToWaypoint();
			this.RotationBasedOnWaypointOrAvoidance();
			this.LimitRotationOfModel();
		}
	}

	// Token: 0x06000016 RID: 22 RVA: 0x00002A54 File Offset: 0x00000E54
	public void OnDisable()
	{
		base.CancelInvoke();
		this._spawner._activeChildren -= 1f;
	}

	// Token: 0x06000017 RID: 23 RVA: 0x00002A74 File Offset: 0x00000E74
	public void OnEnable()
	{
		if (this._instantiated)
		{
			this._spawner._activeChildren += 1f;
			if (this._landing)
			{
				this._model.GetComponent<Animation>().Play(this._spawner._idleAnimation);
			}
			else
			{
				this._model.GetComponent<Animation>().Play(this._spawner._flapAnimation);
			}
		}
	}

	// Token: 0x06000018 RID: 24 RVA: 0x00002AEC File Offset: 0x00000EEC
	public void FindRequiredComponents()
	{
		if (this._thisT == null)
		{
			this._thisT = base.transform;
		}
		if (this._model == null)
		{
			this._model = this._thisT.Find("Model").gameObject;
		}
		if (this._modelT == null)
		{
			this._modelT = this._model.transform;
		}
	}

	// Token: 0x06000019 RID: 25 RVA: 0x00002B64 File Offset: 0x00000F64
	public void RandomizeStartAnimationFrame()
	{
		IEnumerator enumerator = this._model.GetComponent<Animation>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				AnimationState animationState = (AnimationState)obj;
				animationState.time = UnityEngine.Random.value * animationState.length;
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x0600001A RID: 26 RVA: 0x00002BDC File Offset: 0x00000FDC
	public void InitAvoidanceValues()
	{
		this._avoidValue = UnityEngine.Random.Range(0.3f, 0.1f);
		if (this._spawner._birdAvoidDistanceMax != this._spawner._birdAvoidDistanceMin)
		{
			this._avoidDistance = UnityEngine.Random.Range(this._spawner._birdAvoidDistanceMax, this._spawner._birdAvoidDistanceMin);
			return;
		}
		this._avoidDistance = this._spawner._birdAvoidDistanceMin;
	}

	// Token: 0x0600001B RID: 27 RVA: 0x00002C4C File Offset: 0x0000104C
	public void SetRandomScale()
	{
		float num = UnityEngine.Random.Range(this._spawner._minScale, this._spawner._maxScale);
		this._thisT.localScale = new Vector3(num, num, num);
	}

	// Token: 0x0600001C RID: 28 RVA: 0x00002C88 File Offset: 0x00001088
	public void SoarTimeLimit()
	{
		if (this._soar && this._spawner._soarMaxTime > 0f)
		{
			if (this._soarTimer > this._spawner._soarMaxTime)
			{
				this.Flap();
				this._soarTimer = 0f;
			}
			else
			{
				this._soarTimer += this._spawner._newDelta;
			}
		}
	}

	// Token: 0x0600001D RID: 29 RVA: 0x00002CFC File Offset: 0x000010FC
	public void CheckForDistanceToWaypoint()
	{
		if (!this._landing && (this._thisT.position - this._wayPoint).magnitude < this._spawner._waypointDistance + this._stuckCounter)
		{
			this.Wander(0f);
			this._stuckCounter = 0f;
		}
		else if (!this._landing)
		{
			this._stuckCounter += this._spawner._newDelta;
		}
		else
		{
			this._stuckCounter = 0f;
		}
	}

	// Token: 0x0600001E RID: 30 RVA: 0x00002D98 File Offset: 0x00001198
	public void RotationBasedOnWaypointOrAvoidance()
	{
		Vector3 vector = this._wayPoint - this._thisT.position;
		if (this._targetSpeed > -1f && vector != Vector3.zero)
		{
			Quaternion b = Quaternion.LookRotation(vector);
			this._thisT.rotation = Quaternion.Slerp(this._thisT.rotation, b, this._spawner._newDelta * this._damping);
		}
		if (this._spawner._childTriggerPos && (this._thisT.position - this._spawner._posBuffer).magnitude < 1f)
		{
			this._spawner.SetFlockRandomPosition();
		}
		this._speed = Mathf.Lerp(this._speed, this._targetSpeed, this._spawner._newDelta * 2.5f);
		if (this._move)
		{
			this._thisT.position += this._thisT.forward * this._speed * this._spawner._newDelta;
			if (this._avoid && this._spawner._birdAvoid)
			{
				this.Avoidance();
			}
		}
	}

	// Token: 0x0600001F RID: 31 RVA: 0x00002EEC File Offset: 0x000012EC
	public bool Avoidance()
	{
		RaycastHit raycastHit = default(RaycastHit);
		Vector3 forward = this._modelT.forward;
		bool result = false;
		Quaternion rotation = Quaternion.identity;
		Vector3 eulerAngles = Vector3.zero;
		Vector3 position = Vector3.zero;
		position = this._thisT.position;
		rotation = this._thisT.rotation;
		eulerAngles = this._thisT.rotation.eulerAngles;
		if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * this._avoidValue, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.y -= (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			result = true;
		}
		else if (Physics.Raycast(this._thisT.position, forward + this._modelT.right * -this._avoidValue, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.y += (float)this._spawner._birdAvoidHorizontalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			result = true;
		}
		if (this._spawner._birdAvoidDown && !this._landing && Physics.Raycast(this._thisT.position, -Vector3.up, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.x -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			position.y += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = position;
			result = true;
		}
		else if (this._spawner._birdAvoidUp && !this._landing && Physics.Raycast(this._thisT.position, Vector3.up, out raycastHit, this._avoidDistance, this._spawner._avoidanceMask))
		{
			eulerAngles.x += (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * this._damping;
			rotation.eulerAngles = eulerAngles;
			this._thisT.rotation = rotation;
			position.y -= (float)this._spawner._birdAvoidVerticalForce * this._spawner._newDelta * 0.01f;
			this._thisT.position = position;
			result = true;
		}
		return result;
	}

	// Token: 0x06000020 RID: 32 RVA: 0x00003214 File Offset: 0x00001614
	public void LimitRotationOfModel()
	{
		Quaternion localRotation = Quaternion.identity;
		Vector3 eulerAngles = Vector3.zero;
		localRotation = this._modelT.localRotation;
		eulerAngles = localRotation.eulerAngles;
		if ((((this._soar && this._spawner._flatSoar) || (this._spawner._flatFly && !this._soar)) && this._wayPoint.y > this._thisT.position.y) || this._landing)
		{
			eulerAngles.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, -this._thisT.localEulerAngles.x, this._spawner._newDelta * 1.75f);
			localRotation.eulerAngles = eulerAngles;
			this._modelT.localRotation = localRotation;
		}
		else
		{
			eulerAngles.x = Mathf.LerpAngle(this._modelT.localEulerAngles.x, 0f, this._spawner._newDelta * 1.75f);
			localRotation.eulerAngles = eulerAngles;
			this._modelT.localRotation = localRotation;
		}
	}

	// Token: 0x06000021 RID: 33 RVA: 0x00003350 File Offset: 0x00001750
	public void Wander(float delay)
	{
		if (!this._landing)
		{
			this._damping = UnityEngine.Random.Range(this._spawner._minDamping, this._spawner._maxDamping);
			this._targetSpeed = UnityEngine.Random.Range(this._spawner._minSpeed, this._spawner._maxSpeed);
			base.Invoke("SetRandomMode", delay);
		}
	}

	// Token: 0x06000022 RID: 34 RVA: 0x000033B8 File Offset: 0x000017B8
	public void SetRandomMode()
	{
		base.CancelInvoke("SetRandomMode");
		if (!this._dived && UnityEngine.Random.value < this._spawner._soarFrequency)
		{
			this.Soar();
		}
		else if (!this._dived && UnityEngine.Random.value < this._spawner._diveFrequency)
		{
			this.Dive();
		}
		else
		{
			this.Flap();
		}
	}

	// Token: 0x06000023 RID: 35 RVA: 0x0000342C File Offset: 0x0000182C
	public void Flap()
	{
		if (this._move)
		{
			if (this._model != null)
			{
				this._model.GetComponent<Animation>().CrossFade(this._spawner._flapAnimation, 0.5f);
			}
			this._soar = false;
			this.animationSpeed();
			this._wayPoint = this.findWaypoint();
			this._dived = false;
		}
	}

	// Token: 0x06000024 RID: 36 RVA: 0x00003498 File Offset: 0x00001898
	public Vector3 findWaypoint()
	{
		Vector3 zero = Vector3.zero;
		zero.x = UnityEngine.Random.Range(-this._spawner._spawnSphere, this._spawner._spawnSphere) + this._spawner._posBuffer.x;
		zero.z = UnityEngine.Random.Range(-this._spawner._spawnSphereDepth, this._spawner._spawnSphereDepth) + this._spawner._posBuffer.z;
		zero.y = UnityEngine.Random.Range(-this._spawner._spawnSphereHeight, this._spawner._spawnSphereHeight) + this._spawner._posBuffer.y;
		return zero;
	}

	// Token: 0x06000025 RID: 37 RVA: 0x00003548 File Offset: 0x00001948
	public void Soar()
	{
		if (this._move)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
			this._wayPoint = this.findWaypoint();
			this._soar = true;
		}
	}

	// Token: 0x06000026 RID: 38 RVA: 0x00003588 File Offset: 0x00001988
	public void Dive()
	{
		if (this._spawner._soarAnimation != null)
		{
			this._model.GetComponent<Animation>().CrossFade(this._spawner._soarAnimation, 1.5f);
		}
		else
		{
			IEnumerator enumerator = this._model.GetComponent<Animation>().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					AnimationState animationState = (AnimationState)obj;
					if (this._thisT.position.y < this._wayPoint.y + 25f)
					{
						animationState.speed = 0.1f;
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
		}
		this._wayPoint = this.findWaypoint();
		this._wayPoint.y = this._wayPoint.y - this._spawner._diveValue;
		this._dived = true;
	}

	// Token: 0x06000027 RID: 39 RVA: 0x00003684 File Offset: 0x00001A84
	public void animationSpeed()
	{
		IEnumerator enumerator = this._model.GetComponent<Animation>().GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				object obj = enumerator.Current;
				AnimationState animationState = (AnimationState)obj;
				if (!this._dived && !this._landing)
				{
					animationState.speed = UnityEngine.Random.Range(this._spawner._minAnimationSpeed, this._spawner._maxAnimationSpeed);
				}
				else
				{
					animationState.speed = this._spawner._maxAnimationSpeed;
				}
			}
		}
		finally
		{
			IDisposable disposable;
			if ((disposable = (enumerator as IDisposable)) != null)
			{
				disposable.Dispose();
			}
		}
	}

	// Token: 0x04000023 RID: 35
	[HideInInspector]
	public FlockController _spawner;

	// Token: 0x04000024 RID: 36
	[HideInInspector]
	public Vector3 _wayPoint;

	// Token: 0x04000025 RID: 37
	public float _speed;

	// Token: 0x04000026 RID: 38
	[HideInInspector]
	public bool _dived = true;

	// Token: 0x04000027 RID: 39
	[HideInInspector]
	public float _stuckCounter;

	// Token: 0x04000028 RID: 40
	[HideInInspector]
	public float _damping;

	// Token: 0x04000029 RID: 41
	[HideInInspector]
	public bool _soar = true;

	// Token: 0x0400002A RID: 42
	[HideInInspector]
	public bool _landing;

	// Token: 0x0400002B RID: 43
	[HideInInspector]
	public float _targetSpeed;

	// Token: 0x0400002C RID: 44
	[HideInInspector]
	public bool _move = true;

	// Token: 0x0400002D RID: 45
	public GameObject _model;

	// Token: 0x0400002E RID: 46
	public Transform _modelT;

	// Token: 0x0400002F RID: 47
	[HideInInspector]
	public float _avoidValue;

	// Token: 0x04000030 RID: 48
	[HideInInspector]
	public float _avoidDistance;

	// Token: 0x04000031 RID: 49
	private float _soarTimer;

	// Token: 0x04000032 RID: 50
	private bool _instantiated;

	// Token: 0x04000033 RID: 51
	private static int _updateNextSeed;

	// Token: 0x04000034 RID: 52
	private int _updateSeed = -1;

	// Token: 0x04000035 RID: 53
	[HideInInspector]
	public bool _avoid = true;

	// Token: 0x04000036 RID: 54
	public Transform _thisT;

	// Token: 0x04000037 RID: 55
	public Vector3 _landingPosOffset;
}
